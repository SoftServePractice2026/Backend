using AutoMapper;
using Domain.Entities;
using Shared;
using Application.DTOs;
using Domain.Filters;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services.Session;

public class SessionService : ISessionService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<SessionService> _logger;

    public SessionService(
        ISessionRepository sessionRepository,
        ITicketRepository ticketRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<SessionService> logger)
    {
        _sessionRepository = sessionRepository;
        _ticketRepository = ticketRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    
    private SessionListItemDto BuildCard(SessionEntity session) =>
        _mapper.Map<SessionListItemDto>(session);
    
    
    public async Task<Result<Guid>> CreateSessionAsync(SessionCreateDto dto, CancellationToken cancellationToken)
    {
        var hasOverlap = await _sessionRepository.HasOverlapAsync(dto.HallId, dto.StartTime, dto.EndTime, cancellationToken);
        if (hasOverlap)
        {
            return Result<Guid>.Fail(
                Failure.FromError(Error.Conflict("session.overlap",
                    "Session overlaps with an existing session in this hall"))
            );
        }
        var session = _mapper.Map<SessionEntity>(dto);
        
        try
        {
            _sessionRepository.CreateSession(session);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Session created. SessionId={SessionId}", session.Id);
            return Result<Guid>.Success(session.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Create session failed.");
            return Result<Guid>.Fail(Failure.FromError(Error.Internal(message: ex.Message)));
        }
    }
    
    
    public async Task<Result<List<SessionListItemDto>>> GetSessionAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            var sessions = await _sessionRepository.GetSessionEntitiesAsync(cancellationToken);
            var result = _mapper.Map<List<SessionListItemDto>>(sessions);
            return Result<List<SessionListItemDto>>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Get sessions list failed.");
            return Result<List<SessionListItemDto>>.Fail(Failure.FromError(Error.Internal(message: ex.Message)));
        }
    }
    
    
    public async Task<Result<SessionListItemDto>> GetSessionByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var session = await _sessionRepository.GetSessionByIdAsync(id, cancellationToken);
            if (session is null)
            {
                var err = Error.NotFound("session.not.found", $"Session with id: {id} not found");
                _logger.LogWarning("Get session by id not found. SessionId={SessionId}, Code={Code}", id, err.Code);
                return Result<SessionListItemDto>.Fail(Failure.FromError(err));
            }
            return Result<SessionListItemDto>.Success(_mapper.Map<SessionListItemDto>(session));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Get session by id failed. SessionId={SessionId}", id);
            return Result<SessionListItemDto>.Fail(Failure.FromError(Error.Internal(message: ex.Message)));
        }
    }
    

    public async Task<Result<SessionListItemDto>> UpdateSessionAsync(
        Guid targetId,
        SessionUpdateDto dto,
        CancellationToken cancellationToken) {
        var session = await _sessionRepository.GetSessionByIdAsync(targetId, cancellationToken);
        if (session is null)
        {
            var err = Error.NotFound("session.not.found", $"Session with id: {targetId} not found");
            _logger.LogWarning("Update session not found. SessionId={SessionId}, Code={Code}", targetId, err.Code);
            return Result<SessionListItemDto>.Fail(Failure.FromError(err));
        }
        try
        {
            _mapper.Map(dto, session);

            _sessionRepository.UpdateSession(session);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<SessionListItemDto>.Success(BuildCard(session));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Update session failed. SessionId={SessionId}", targetId);
            return Result<SessionListItemDto>.Fail(Failure.FromError(Error.Internal(message: ex.Message)));
        }
    }
    
    
    public async Task DeleteSessionAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var session = await _sessionRepository.GetSessionByIdAsync(id, cancellationToken);
            if (session is null)
            {
                _logger.LogInformation("Delete session: already absent. SessionId={SessionId}", id);
                return;
            }
            _sessionRepository.DeleteSession(session);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Session deleted. SessionId={SessionId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Delete session failed. SessionId={SessionId}", id);
            throw;
        }
    }
    

    public async Task<Result<SessionFilterResultDto>> GetFilteredSessionsAsync(SessionFilterDto dto, CancellationToken ct)
    {
        var filter = new SessionFilter(
            dto.MovieId,
            dto.HallId,
            dto.Status,
            dto.From,
            dto.To,
            dto.MovieTitle,
            dto.PageNumber ?? 1,
            dto.PageSize ?? 10
        );
        
        var sessions = await _sessionRepository.GetFilteredSessionsAsync(filter, ct);
        var total = await _sessionRepository.CountFilteredAsync(filter, ct);
        var cards = sessions.Select(BuildCard).ToList();
        return Result<SessionFilterResultDto>.Success(new SessionFilterResultDto(cards, total));
    }
}