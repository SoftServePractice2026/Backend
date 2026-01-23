using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly CinemaDbContext _context;

        public GenreRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task CreateGenreAsync(GenreEntity genreEntity)
        {
            await _context.Genres.AddAsync(genreEntity);
        }

        public Task DeleteGenreAsync(GenreEntity genreEntity)
        {
            _context.Genres.Remove(genreEntity);
            return Task.CompletedTask;
        }

        public Task<GenreEntity?> GetGenreByIdAsync(Guid genreId) => _context.Genres.FirstOrDefaultAsync(x => x.Id == genreId);


        public async Task<GenreEntity> GetGenreByNameAsync(string name)
        {
            return await _context.Genres
        .AsNoTracking()
        .Where(a => EF.Functions.ILike(a.Name, $"{name}%"))
        .FirstOrDefaultAsync();
        }
        

        public async Task<IReadOnlyList<GenreEntity>> GetGenreEntitiesAsync() => await _context.Genres.ToListAsync();
        

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
         

        public Task UpdateGenreAsync(GenreEntity genreEntity)
        {
            _context.Genres.Update(genreEntity);
            return Task.CompletedTask;
        }
    }
}
