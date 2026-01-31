using Domain.Entities;
using Domain.Filters;

namespace Domain.Interfaces
{
    public interface IViewHistoryRepository
    {
        void CreateViewHistory(ViewHistoryEntity viewHistoryEntity);
        void DeleteViewHistory(ViewHistoryEntity viewHistoryEntity);
        void UpdateViewHistory(ViewHistoryEntity viewHistoryEntity);

        Task<ViewHistoryEntity?> GetViewHistoryByIdAsync(Guid viewHistoryId, CancellationToken cancellationToken);

        Task<List<ViewHistoryEntity>> GetFilteredViewHistoryAsync(ViewHistoryFilter viewHistoryFilter,
            CancellationToken cancellationToken);

        Task<int> CountFilteredAsync(ViewHistoryFilter viewHistoryFilter, CancellationToken cancellationToken);
    }
}
