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

        public Task DeleteHallAsync(HallEntity hallEntity)
        {
            _context.Halls.Remove(hallEntity);
            return Task.CompletedTask;
        }

        public Task<HallEntity?> GetHallByIdAsync(Guid hallId) => _context.Halls.FirstOrDefaultAsync(x => x.Id == hallId);

        public Task<HallEntity?> GetHallByNameAsync(string hallName) => _context.Halls.FirstOrDefaultAsync(x => x.Name == hallName);

        public Task<List<HallEntity>> GetHallEntitiesAsync() => _context.Halls.ToListAsync();

        public Task UpdateHallAsync(HallEntity hallEntity)
        {
            _context.Halls.Update(hallEntity);
            return Task.CompletedTask;
        }

        public async Task CreateHallAsync(HallEntity hallEntity)
        {
            await _context.Halls.AddAsync(hallEntity);
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
