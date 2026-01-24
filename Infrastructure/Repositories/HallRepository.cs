using Domain.Entities;
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

        public async Task<List<HallEntity>> GetHallsAsync(CancellationToken cancellationToken) => 
            await _context.Halls.ToListAsync(cancellationToken);

        public async Task<HallEntity?> GetHallByIdAsync(Guid hallId, CancellationToken cancellationToken) => 
            await _context.Halls.FindAsync([hallId], cancellationToken);

        public async Task<HallEntity?> GetHallByNameAsync(string hallName, CancellationToken cancellationToken) =>
            await _context.Halls.FirstOrDefaultAsync(h => h.Name == hallName, cancellationToken);
    }
}
