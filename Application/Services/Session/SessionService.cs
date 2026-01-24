using AutoMapper;
using Domain.Entities;
using Domain.Entities.Enums;
using Shared;
using Application.DTOs;
using Domain.Interfaces;


namespace Application.Services.Session;



public class SessionService : ISessionService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IMapper _mapper;
    
    public SessionService(
        ISessionRepository sessionRepository,
        ITicketRepository ticketRepository,
        IMapper mapper)
    {
        _sessionRepository = sessionRepository;
        _ticketRepository = ticketRepository;
        _mapper = mapper;
    }
    
    private SessionDtos.SessionCardDto BuildCard(SessionEntity session, decimal price) =>
        _mapper.Map<SessionDtos.SessionCardDto>(session) with
        {
            Price = price,
            Seats = new List<SessionDtos.SeatInSessionDto>()
        };
    
    
    
    public async Task<Result<Guid>> CreateSessionAsync(SessionDtos.SessionCreateDto dto)
    {
        var session = _mapper.Map<SessionEntity>(dto);
        try
        {
            await _sessionRepository.CreateSessionAsync(session);
            await _sessionRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result<Guid>.Fail(
                Failure.FromError(Error.Internal(message: ex.Message)));
        }
        return Result<Guid>.Success(session.Id);
    }
    
    
    
    public async Task<Result<List<SessionDtos.SessionCardDto>>> GetSessionAllAsync()
    {
        try
        {
            var sessions = await _sessionRepository.GetSessionEntitiesAsync();
            var result = _mapper.Map<List<SessionDtos.SessionCardDto>>(sessions);
            return Result<List<SessionDtos.SessionCardDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<List<SessionDtos.SessionCardDto>>.Fail(
                Failure.FromError(Error.Internal(message: ex.Message)));
        }
    }
    
    
    
    public async Task<Result<SessionDtos.SessionCardDto>> GetSessionByIdAsync(Guid id)
    {
        var session = await _sessionRepository.GetSessionByIdAsync(id);
        if (session is null)
        {
            return Result<SessionDtos.SessionCardDto>.Fail(
                Failure.FromError(Error.NotFound("session.not.found", $"Session with id: {id} not found")));
        }
        var dto = _mapper.Map<SessionDtos.SessionCardDto>(session);
        return Result<SessionDtos.SessionCardDto>.Success(dto);
    }

    
    
    public async Task<Result<SessionDtos.SessionCardDto>> UpdateSessionAllAsync(Guid targetId, SessionDtos.SessionUpdateAllDto dto)
    {
        var session = await _sessionRepository.GetSessionByIdAsync(targetId);
        if (session is null)
        {
            return Result<SessionDtos.SessionCardDto>.Fail(
                Failure.FromError(Error.NotFound("session.not.found", $"Session with id: {targetId} not found")));
        }

        try
        {
            _mapper.Map(dto, session);
            await _sessionRepository.UpdateSessionAsync(session);
            await _ticketRepository.UpdatePriceForActiveTicketsBySessionIdAsync(targetId, dto.Price);
            await _sessionRepository.SaveChangesAsync();
            
            return Result<SessionDtos.SessionCardDto>.Success(BuildCard(session, dto.Price));

        } 
        catch (Exception ex)
        {
            return Result<SessionDtos.SessionCardDto>.Fail(
                Failure.FromError(Error.Internal(message: ex.Message)));
        }
    }


    
    public async Task<Result<SessionDtos.SessionCardDto>> UpdateSessionTimeAsync(Guid targetId, SessionDtos.SessionUpdateTimeDto dto)
    {
        var session = await _sessionRepository.GetSessionByIdAsync(targetId);
        if (session is null)
        {
            return Result<SessionDtos.SessionCardDto>.Fail(
                Failure.FromError(Error.NotFound("session.not.found", $"Session with id: {targetId} not found")));
        }

        try
        {
            _mapper.Map(dto, session);
            await _sessionRepository.UpdateSessionAsync(session);
            await _sessionRepository.SaveChangesAsync();

            var outDto = _mapper.Map<SessionDtos.SessionCardDto>(session) with
            {
                Price = 0m,
                Seats = new List<SessionDtos.SeatInSessionDto>()
            };

            return Result<SessionDtos.SessionCardDto>.Success(outDto);
        } catch (Exception ex)
        {
            return Result<SessionDtos.SessionCardDto>.Fail(
                Failure.FromError(Error.Internal(message: ex.Message)));
        }
    }

    
    
    public async Task<Result<SessionDtos.SessionCardDto>> UpdateSessionStatusAsync(Guid targetId, SessionDtos.SessionUpdateStatusDto dto)
    {
        var session = await _sessionRepository.GetSessionByIdAsync(targetId);
        if (session is null)
        {
            return Result<SessionDtos.SessionCardDto>.Fail(
                Failure.FromError(Error.NotFound("session.not.found", $"Session with id: {targetId} not found")));
        }
        
        try
        {
            _mapper.Map(dto, session);

            await _sessionRepository.UpdateSessionAsync(session);
            await _sessionRepository.SaveChangesAsync();
            var outDto = _mapper.Map<SessionDtos.SessionCardDto>(session) with
            {
                Price = 0m,
                Seats = new List<SessionDtos.SeatInSessionDto>()
            };
            return Result<SessionDtos.SessionCardDto>.Success(outDto);
        }
        
        catch (Exception ex)
        {
            return Result<SessionDtos.SessionCardDto>.Fail(
                Failure.FromError(Error.Internal(message: ex.Message)));
        }
    }
    
    
    public async Task<Result<SessionDtos.SessionCardDto>> UpdateSessionPriceAsync(Guid targetId, SessionDtos.SessionUpdatePriceDto dto)
    {
        var session = await _sessionRepository.GetSessionByIdAsync(targetId);
        if (session is null)
        {
            return Result<SessionDtos.SessionCardDto>.Fail(
                Failure.FromError(Error.NotFound("session.not.found", $"Session with id: {targetId} not found")));
        }
        try
        {
            await _ticketRepository.UpdatePriceForActiveTicketsBySessionIdAsync(targetId, dto.Price);
            return Result<SessionDtos.SessionCardDto>.Success(BuildCard(session, dto.Price));
        }
        catch (Exception ex)
        {
            return Result<SessionDtos.SessionCardDto>.Fail(
                Failure.FromError(Error.Internal(message: ex.Message)));
        }
    }
    
    
    
    public async Task<Result<bool>> DeleteSessionAsync(Guid id)
    {
        var session = await _sessionRepository.GetSessionByIdAsync(id);
        if (session is null) 
        {
            return Result<bool>.Fail(
                Failure.FromError(Error.NotFound("session.not.found", $"Session with id: {id} not found")));
        }
        
        try
        {
            await _sessionRepository.DeleteSessionAsync(session);
            await _sessionRepository.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail(
                Failure.FromError(Error.Internal(message: ex.Message)));
        }
    }
}