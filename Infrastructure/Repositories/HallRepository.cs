using Domain.Entities;
using Domain.Entities.Extensions;
using Domain.Filters;
using Domain.Interfaces;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class HallRepository : IHallRepository
    {
        private readonly CinemaDbContext _context;
        private readonly ILogger<HallRepository> _logger;

        public HallRepository(CinemaDbContext context, ILogger<HallRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void CreateHall(HallEntity hallEntity) => _context.Halls.Add(hallEntity);
        public void DeleteHall(HallEntity hallEntity) => _context.Halls.Remove(hallEntity);
        public void UpdateHall(HallEntity hallEntity) => _context.Halls.Update(hallEntity);
        public async Task<bool> ExistsAsync(Guid hallId, CancellationToken cancellationToken)
        {
            return await _context.Halls.AnyAsync(e => e.Id == hallId, cancellationToken);
        }

        public async Task<HallEntity?> GetHallByIdAsync(Guid hallId, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Halls.FindAsync([hallId], cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, $"Db error while getting hall by id: {hallId}");
                throw;
            }
        }
        public async Task<HallEntity?> GetHallByNameAsync(string hallName, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Halls.FirstOrDefaultAsync(h => h.Name == hallName, cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, $"Db error while getting hall by name: {hallName}");
                throw;
            }
        }

        public async Task<List<HallEntity>> GetFilteredHallsAsync(HallFilter hallFilter, CancellationToken cancellationToken)
        {
            var query = _context.Halls.AsQueryable();

            //Filters
            if (hallFilter.IsActive.HasValue)
                query = query.Where(h => h.IsActive == hallFilter.IsActive.Value);

            if (hallFilter.HallSize.HasValue)
                query = query.Where(h => h.HallSize == hallFilter.HallSize.Value);

            //Sorting
            query = query.ApplyOrderBy(
                hallFilter.OrderBy,
                hallFilter.SortDirection,
                HallOrderByMap.Map);

            //Pagination
            var skip = (hallFilter.PageNumber - 1) * hallFilter.PageSize;
            query = query.Skip(skip).Take(hallFilter.PageSize);

            try
            {
                return await query.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Database error while filtering halls");
                throw;
            }
        }

        public async Task<int> CountFilteredAsync(HallFilter hallFilter, CancellationToken cancellationToken)
        {
            var query = _context.Halls.AsQueryable();

            if (hallFilter.IsActive.HasValue)
                query = query.Where(h => h.IsActive == hallFilter.IsActive.Value);

            if (hallFilter.HallSize.HasValue)
                query = query.Where(h => h.HallSize == hallFilter.HallSize.Value);

            try
            {
                return await query.CountAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Database error while count filtered halls");
                throw;
            }
        }
    }
}
