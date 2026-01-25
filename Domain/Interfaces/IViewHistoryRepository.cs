using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IViewHistoryRepository
    {
        Task CreateViewHistoryAsync(ViewHistoryEntity viewHistoryEntity);
        
        Task DeleteViewHistoryAsync(ViewHistoryEntity viewHistoryEntity);
        
        Task UpdateViewHistoryAsync(ViewHistoryEntity viewHistoryEntity);
        
        Task<ViewHistoryEntity?> GetViewHistoryByIdAsync(Guid viewHistoryId);
        
        Task<List<ViewHistoryEntity>> GetViewHistoryEntitiesAsync();
        
        Task SaveChangesAsync();
    }
}