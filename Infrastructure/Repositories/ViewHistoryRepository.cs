using Domain.Entities;
using Domain.Entities.Extensions;
using Domain.Filters;
using Domain.Interfaces;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class ViewHistoryRepository : IViewHistoryRepository
    {
        private readonly CinemaDbContext _context;
        private readonly ILogger<ViewHistoryRepository> _logger;

        public ViewHistoryRepository(CinemaDbContext context, ILogger<ViewHistoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public void CreateViewHistory(ViewHistoryEntity viewHistoryEntity) => _context.ViewHistories.Add(viewHistoryEntity);
        public void DeleteViewHistory(ViewHistoryEntity viewHistoryEntity) => _context.ViewHistories.Remove(viewHistoryEntity);
        public void UpdateViewHistory(ViewHistoryEntity viewHistoryEntity) => _context.ViewHistories.Update(viewHistoryEntity);

        public async Task<ViewHistoryEntity?> GetViewHistoryByIdAsync(Guid viewHistoryId, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.ViewHistories.FindAsync([viewHistoryId], cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, $"Db error while getting view history by id: {viewHistoryId}");
                throw;
            }
        }

        public async Task<List<ViewHistoryEntity>> GetFilteredViewHistoryAsync(ViewHistoryFilter viewHistoryFilter, CancellationToken cancellationToken)
        {
            var query = _context.ViewHistories.AsQueryable();
            
            if (viewHistoryFilter.UserId.HasValue)
                query = query.Where(v => v.UserId == viewHistoryFilter.UserId.Value);

            if (viewHistoryFilter.SessionId.HasValue)
                query = query.Where(v => v.SessionId == viewHistoryFilter.SessionId.Value);
            
            query = query.ApplyOrderBy(
                viewHistoryFilter.OrderBy,
                viewHistoryFilter.SortDirection,
                ViewHistoryOrederByMap.Map);
            
            var skip = (viewHistoryFilter.PageNumber - 1) * viewHistoryFilter.PageSize;
            query = query.Skip(skip).Take(viewHistoryFilter.PageSize);

            try
            {
                return await query.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error while filtering view history");
                throw;
            }
        }

        
        
        public async Task<int> CountFilteredAsync(ViewHistoryFilter viewHistoryFilter, CancellationToken cancellationToken)
        {
            var query = _context.ViewHistories.AsQueryable();

            if (viewHistoryFilter.UserId.HasValue)
                query = query.Where(v => v.UserId == viewHistoryFilter.UserId.Value);

            if (viewHistoryFilter.SessionId.HasValue)
                query = query.Where(v => v.SessionId == viewHistoryFilter.SessionId.Value);

            try
            {
                return await query.CountAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error while count filtered view history");
                throw;
            }
        }
    }
}