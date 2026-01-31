using Domain.Entities;
using Domain.Entities.Extensions;
using Domain.Filters;
using Domain.Interfaces;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Infrastructure.Repositories;
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
    
    
    
    public async Task<bool> HasOverlapAsync(Guid hallId, DateTime start, DateTime end, CancellationToken ct)
    {
        return await _context.Sessions
            .AsNoTracking()
            .AnyAsync(s =>
                    s.HallId == hallId &&
                    start < s.EndTime &&
                    end > s.StartTime,
                ct);
    }
    
    
    public async Task<List<SessionEntity>> GetFilteredSessionsAsync(SessionFilter filter, CancellationToken cancellationToken)
    {
        var query = _context.Sessions
            .AsNoTracking()
            .Include(s => s.Movie)
            .Include(s => s.Hall)
            .AsQueryable();

        query = query.ApplyFilters(filter);

        query = query.ApplyOrderBy(
            filter.OrderBy,
            filter.SortDirection,
            SessionOrderByMap.Map);

        var skip = (filter.PageNumber - 1) * filter.PageSize;
        query = query.Skip(skip).Take(filter.PageSize);

        try
        {
            return await query.ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database error while filtering sessions");
            throw;
        }
    }

    
    public async Task<int> CountFilteredAsync(SessionFilter filter, CancellationToken cancellationToken)
    {
        var query = _context.Sessions
            .Include(s => s.Movie)
            .AsQueryable();

        query = query.ApplyFilters(filter);
        try
        {
            return await query.CountAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database error while count filtered sessions");
            throw;
        }
    }

}

