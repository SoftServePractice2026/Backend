using Domain.Entities;

namespace Domain.Interfaces;

public interface ISessionRepository
{
    void CreateSession(SessionEntity sessionEntity);
    void DeleteSession(SessionEntity sessionEntity);
    void UpdateSession(SessionEntity sessionEntity);

    
    Task<SessionEntity?> GetSessionByIdAsync(Guid sessionId, CancellationToken cancellationToken);
    Task<List<SessionEntity>> GetSessionEntitiesAsync(CancellationToken ct);
    Task<bool> HasOverlapAsync(Guid hallId, DateTime start, DateTime end, CancellationToken ct);
}
