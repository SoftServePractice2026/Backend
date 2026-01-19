using Domain.Entities;
using Domain.Interfaces;
using System;

namespace Infrastructure.Repositories
{
    public class HallRepository : IHallRepository
    {
        private readonly CinemaDbContext _context;

        public HallRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<Guid?> CreateHallAsync(HallEntity hallEntity)
        {
            await _context.Halls.AddAsync(hallEntity);
            await _context.SaveChangesAsync();

            return hallEntity.Id;
        }
    }
}
