using Domain.Entities;
using Domain.Filters;

namespace Domain.Interfaces;

public interface ISessionRepository
{
    void CreateSession(SessionEntity sessionEntity);
    void DeleteSession(SessionEntity sessionEntity);
    void UpdateSession(SessionEntity sessionEntity);

    
    Task<SessionEntity?> GetSessionByIdAsync(Guid sessionId, CancellationToken cancellationToken);
    Task<bool> HasOverlapAsync(Guid hallId, DateTime start, DateTime end, CancellationToken ct);
    
    Task<List<SessionEntity>> GetFilteredSessionsAsync(SessionFilter filter, CancellationToken ct);
    Task<int> CountFilteredAsync(SessionFilter filter, CancellationToken ct);
}
