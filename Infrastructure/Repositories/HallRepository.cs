using Domain.Entities;
using Domain.Filters;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class HallRepository : IHallRepository
    {
        private readonly CinemaDbContext _context;

        public HallRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public void CreateHall(HallEntity hallEntity) => _context.Halls.Add(hallEntity);
        public void DeleteHall(HallEntity hallEntity) => _context.Halls.Remove(hallEntity);
        public void UpdateHall(HallEntity hallEntity) => _context.Halls.Update(hallEntity);

        public async Task<HallEntity?> GetHallByIdAsync(Guid hallId, CancellationToken cancellationToken) =>
            await _context.Halls.FindAsync([hallId], cancellationToken);
        public async Task<HallEntity?> GetHallByNameAsync(string hallName, CancellationToken cancellationToken) =>
            await _context.Halls.FirstOrDefaultAsync(h => h.Name == hallName, cancellationToken);

        public async Task<List<HallEntity>> GetFilteredHallsAsync(HallFilter hallFilter, CancellationToken cancellationToken)
        {
            var query = _context.Halls.AsQueryable();

            if (hallFilter.IsActive.HasValue)
                query = query.Where(h => h.IsActive == hallFilter.IsActive.Value);

            if (hallFilter.HallSize.HasValue)
                query = query.Where(h => h.HallSize == hallFilter.HallSize.Value);

            var skip = (hallFilter.Page - 1) * hallFilter.PageSize;
            query = query.Skip(skip).Take(hallFilter.PageSize);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<int> CountFilteredAsync(HallFilter hallFilter, CancellationToken cancellationToken)
        {
            var query = _context.Halls.AsQueryable();

            if (hallFilter.IsActive.HasValue)
                query = query.Where(h => h.IsActive == hallFilter.IsActive.Value);

            if (hallFilter.HallSize.HasValue)
                query = query.Where(h => h.HallSize == hallFilter.HallSize.Value);

            return await query.CountAsync(cancellationToken);
        }
    }
}
