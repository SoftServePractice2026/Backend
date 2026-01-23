using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ViewHistoryRepository : IViewHistoryRepository
    {
        private readonly CinemaDbContext _context;
        public ViewHistoryRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task CreateViewHistoryAsync(ViewHistoryEntity viewHistoryEntity)
        {
            await _context.ViewHistories.AddAsync(viewHistoryEntity);
        }

        public Task DeleteViewHistoryAsync(ViewHistoryEntity viewHistoryEntity)
        {
            _context.ViewHistories.Remove(viewHistoryEntity);
            return Task.CompletedTask;
        }
        
        public Task UpdateViewHistoryAsync(ViewHistoryEntity viewHistoryEntity)
        {
            _context.ViewHistories.Update(viewHistoryEntity);
            return Task.CompletedTask;
        }

        public Task<ViewHistoryEntity?> GetViewHistoryByIdAsync(Guid viewHistoryId) => _context.ViewHistories.FirstOrDefaultAsync(x => x.Id == viewHistoryId);
        public Task<List<ViewHistoryEntity>> GetViewHistoryEntitiesAsync() => _context.ViewHistories.ToListAsync();
        
        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}