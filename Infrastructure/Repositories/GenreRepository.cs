using Domain.Entities;
using Domain.Entities.Extensions;
using Domain.Filters;
using Domain.Interfaces;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

        public async Task<List<GenreEntity>> GetAllGenresAsync(GenreFilter filter, CancellationToken cancellationToken)
        {
            var query = _context.Genres.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(x => x.Name.Contains(filter.Name.Trim()));
            }

            query = query.ApplyOrderBy(
                filter.OrderBy,
                filter.SortDirection,
                GenreOrederByMap.Map);

            var skip = (filter.PageNumber - 1) * filter.PageSize;
            query = query.Skip(skip).Take(filter.PageSize);

            try
            {
                return await query.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Database error while filtering genres");
                throw;
            }
        }

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
