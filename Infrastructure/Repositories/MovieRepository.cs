using Application.Services.Movie.MovieRepository;
using Domain.Entities;
using Domain.Filters;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly CinemaDbContext _dbContext;
    private readonly ILogger<MovieRepository> _logger;

    public MovieRepository(CinemaDbContext dbContext, ILogger<MovieRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public void AddMovie(MovieEntity movieEntity) => _dbContext.Movies.Add(movieEntity);
    public void UpdateMovie(MovieEntity movieEntity) => _dbContext.Movies.Update(movieEntity);
    public void DeleteMovie(MovieEntity movieEntity) => _dbContext.Movies.Remove(movieEntity);
    
    public async Task<MovieEntity?> GetMovieByIdAsync(Guid movieId, CancellationToken cancellationToken)
    {
        return await _dbContext.Movies
            .Include(m => m.Genres)
            .Include(m => m.ActorsInMovies)
            .FirstOrDefaultAsync(a => a.Id == movieId);
    }

    public async Task<MovieEntity?> GetMovieByNameAsync(string movieName, CancellationToken cancellationToken)
    {
        return await _dbContext.Movies.FirstOrDefaultAsync(a => a.Title == movieName);
    }

    public async Task<List<MovieEntity>> GetFilteredMoviesAsync(MovieFilter movieFilter, CancellationToken cancellationToken)
    {
        var query = _dbContext.Movies
            .Include(m => m.Genres)
            .AsQueryable();

        if (!string.IsNullOrEmpty(movieFilter.Title))
        {
            query = query.Where(m => m.Title.Contains(movieFilter.Title));
        }

        if (movieFilter.GenreId.HasValue)
        {
            query = query.Where(m => m.Genres.Any(g => g.Id == movieFilter.GenreId.Value));
        }

        if (movieFilter.MinAgeRating.HasValue)
        {
            query = query.Where(m => m.AgeRating >= movieFilter.MinAgeRating.Value);
        }

        var skip = (movieFilter.Page - 1) * movieFilter.PageSize;
        
        return await query.Skip(skip).Take(movieFilter.PageSize).ToListAsync(cancellationToken);
    }

    public async Task<int> CountFilteredAsync(MovieFilter movieFilter, CancellationToken cancellationToken)
    {
        var query = _dbContext.Movies.AsQueryable();

        if (!string.IsNullOrEmpty(movieFilter.Title))
        {
            query = query.Where(m => m.Title.Contains(movieFilter.Title));
        }

        if (movieFilter.GenreId.HasValue)
        {
            query = query.Where(m => m.Genres.Any(g => g.Id == movieFilter.GenreId.Value));
        }

        if (movieFilter.MinAgeRating.HasValue)
        {
            query = query.Where(m => m.AgeRating >= movieFilter.MinAgeRating.Value);
        }
        
        return await query.CountAsync(cancellationToken);
        
    }
}