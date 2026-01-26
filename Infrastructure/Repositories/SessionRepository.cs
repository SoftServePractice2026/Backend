namespace Infrastructure.Repositories;

using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


public class SessionRepository : ISessionRepository
{
    private readonly CinemaDbContext _context;
    private readonly ILogger<SessionRepository> _logger;

    public SessionRepository(CinemaDbContext context, ILogger<SessionRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public void CreateSession(SessionEntity sessionEntity) => _context.Sessions.Add(sessionEntity);
    public void DeleteSession(SessionEntity sessionEntity) => _context.Sessions.Remove(sessionEntity);
    public void UpdateSession(SessionEntity sessionEntity) => _context.Sessions.Update(sessionEntity);
    
    
    public async Task<SessionEntity?> GetSessionByIdAsync(Guid sessionId, CancellationToken cancellationToken)
    {
        try
        {
            return await _context.Sessions
                .AsNoTracking()
                .Include(s => s.Movie)
                .Include(s => s.Hall)
                .Include(s => s.Tickets)
                .FirstOrDefaultAsync(s => s.Id == sessionId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Db error while getting session by id: {SessionId}", sessionId);
            throw;
        }
    }
    
    public async Task<List<SessionEntity>> GetSessionEntitiesAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _context.Sessions
                .AsNoTracking()
                .Include(s => s.Movie)
                .Include(s => s.Hall)
                .Include(s => s.Tickets) // якщо з них дістаєш Price / Seats
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Db error while getting sessions list");
            throw;
        }
    }
    
    
    public async Task<bool> HasOverlapAsync(Guid hallId, DateTime start, DateTime end, CancellationToken ct)
    {
        return await _context.Sessions.AnyAsync(s =>
                s.HallId == hallId &&
                start < s.EndTime &&
                end > s.StartTime,
            ct);
    }
}

