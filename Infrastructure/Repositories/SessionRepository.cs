namespace Infrastructure.Repositories;

using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;


public class SessionRepository : ISessionRepository
{
    private readonly CinemaDbContext _context;

    public SessionRepository(CinemaDbContext context)
    {
        _context = context;
    }

    public async Task CreateSessionAsync(SessionEntity sessionEntity)
    {
        await _context.Sessions.AddAsync(sessionEntity);
    }
    
    public Task DeleteSessionAsync(SessionEntity sessionEntity)
    {
        _context.Sessions.Remove(sessionEntity);
        return Task.CompletedTask;
    }

    public Task UpdateSessionAsync(SessionEntity sessionEntity)
    {
        _context.Sessions.Update(sessionEntity);
        return Task.CompletedTask;
    }

    
    public Task<SessionEntity?> GetSessionByIdAsync(Guid sessionId)
        => _context.Sessions.FirstOrDefaultAsync(x => x.Id == sessionId);

    
    public Task<List<SessionEntity>> GetSessionEntitiesAsync()
        => _context.Sessions.ToListAsync();

    
    public Task SaveChangesAsync()
        => _context.SaveChangesAsync();
}

