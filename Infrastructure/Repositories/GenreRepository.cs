using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly CinemaDbContext _context;
        private readonly ILogger<GenreRepository> _logger;

        public GenreRepository(CinemaDbContext context, ILogger<GenreRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void CreateGenre(GenreEntity genreEntity) => _context.Genres.Add(genreEntity);
        

        public void DeleteGenre(GenreEntity genreEntity) => _context.Genres.Remove(genreEntity);


        public async Task<List<GenreEntity>> GetAllGenresAsync(CancellationToken cancellationToken) => await _context.Genres.ToListAsync(cancellationToken);
       
       

        public async Task<GenreEntity?> GetGenreByIdAsync(Guid genreId, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Genres.FindAsync([genreId], cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, $"Db error while getting genre by id: {genreId}");
                throw;
            }
        }

        public async Task<GenreEntity?> GetGenreByNameAsync(string name, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Genres.FirstOrDefaultAsync(h => h.Name == name, cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, $"Db error while getting hall by name: {name}");
                throw;
            }
        }

        public void UpdateGenre(GenreEntity genreEntity) => _context.Genres.Update(genreEntity);

    }
}
