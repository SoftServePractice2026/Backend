using Domain.Entities;

namespace Domain.Interfaces;

public interface ISessionRepository
{
    Task CreateSessionAsync(SessionEntity sessionEntity);
    
    Task DeleteSessionAsync(SessionEntity sessionEntity);
    
    Task UpdateSessionAsync(SessionEntity sessionEntity);
    
    Task<SessionEntity?> GetSessionByIdAsync(Guid sessionId);
    
    Task<List<SessionEntity>> GetSessionEntitiesAsync();
    
    Task SaveChangesAsync();
}