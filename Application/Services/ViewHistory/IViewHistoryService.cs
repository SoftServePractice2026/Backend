using Application.DTOs;
using Shared;

namespace Application.Services.ViewHistory
{
    public interface IViewHistoryService
    {
        Task<Result<ViewHistoryDetailsDto>> CreateViewHistoryAsync(ViewHistoryCreateDto dto, CancellationToken cancellationToken);
        
        Task<Result<ViewHistoryDetailsDto>> UpdateViewHistoryAsync(Guid targetId, ViewHistoryUpdateDto dto, CancellationToken cancellationToken);
        
        Task<Result<bool>> DeleteViewHistoryAsync(Guid id, CancellationToken cancellationToken);
        
        Task<Result<ViewHistoryDetailsDto>> GetViewHistoryByIdAsync(Guid id, CancellationToken cancellationToken);
        
        Task<Result<List<ViewHistoryListItemDto>>> GetViewHistoryByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        
        Task<Result<(List<ViewHistoryListItemDto> Items, int TotalCount)>> GetFilteredViewHistoryAsync(ViewHistoryFilterDto filterDto, CancellationToken cancellationToken);
    }
}