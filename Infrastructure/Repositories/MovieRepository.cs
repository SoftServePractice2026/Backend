using Application.Services.Movie.MovieRepository;
using Domain.Entities;
using Domain.Entities.Extensions;
using Domain.Filters;
using Domain.Interfaces;
using Infrastructure.Extensions;
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


    public async Task<MovieEntity?> GetByTmdbIdAsync(int tmdbId)
    {
        return await _dbContext.Movies
            .Include(m => m.Genres) 
            .Include(m => m.ActorsInMovies).ThenInclude(am => am.Actor) 
            .FirstOrDefaultAsync(m => m.TmdbId == tmdbId);
    }

    public void AddActorsToMovie(MovieEntity movieEntity, List<Guid> actorsIds)
    {
        var newActorLinks =
            actorsIds
                .Where(id => movieEntity.ActorsInMovies.All(am => am.ActorId != id))
                .Select(a => new MovieActorEntity
            {
                MovieId = movieEntity.Id, 
                ActorId = a
            })
            .ToList();

        if (newActorLinks.Any())
        {
            _dbContext.MovieActors.AddRange(newActorLinks);
        }
    }

    public void AddGenresToMovie(MovieEntity movieEntity, List<Guid> genresIds)
    {
        var newGenres = genresIds
            .Where(id => movieEntity.Genres.All(mg => mg.Id != id))
            .ToList();

        foreach (var id in newGenres)
        {
            var genreStub = new GenreEntity { Id = id };
            
            _dbContext.Genres.Attach(genreStub);
            
            movieEntity.Genres.Add(genreStub);
        }
    }
    
    
    
    

    public void DeleteActorsFromMovie(MovieEntity movieEntity, List<Guid> actorsIds)
    {
        var actorsToRemove = movieEntity.ActorsInMovies
            .Where(a => actorsIds.Contains(a.ActorId))
            .ToList();
        
        _dbContext.MovieActors.RemoveRange(actorsToRemove);
    }

    public void DeleteGenresFromMovie(MovieEntity movieEntity, List<Guid> genresIds)
    {
        var toRemove = movieEntity.Genres
            .Where(g => genresIds.Contains(g.Id))
            .ToList();

        foreach (var genre in toRemove)
        {
            movieEntity.Genres.Remove(genre);
        }
    }

    public async Task<MovieEntity?> GetMovieByIdAsync(Guid movieId, CancellationToken cancellationToken)
    {
        return await _dbContext.Movies
            .Include(m => m.Genres)
            .Include(m => m.ActorsInMovies)
            .ThenInclude(am => am.Actor)
            .FirstOrDefaultAsync(a => a.Id == movieId, cancellationToken);
    }

    public async Task<MovieEntity?> GetMovieByNameAsync(string movieName, CancellationToken cancellationToken)
    {
        return await _dbContext.Movies.FirstOrDefaultAsync(a => a.Title == movieName);
    }

    public async Task<List<MovieEntity>> GetFilteredMoviesAsync(MovieFilter movieFilter, CancellationToken cancellationToken)
    {
        var query = _dbContext.Movies
            .Include(m => m.Genres)
            .Include(x => x.ActorsInMovies)
            .AsQueryable();

        if (movieFilter.ActorsIds != null && movieFilter.ActorsIds.Any())
        {
            query = query.Where(m => m.ActorsInMovies.Any(g => movieFilter.ActorsIds.Contains(g.Id)));
        }


        if (!string.IsNullOrEmpty(movieFilter.Title))
        {
            query = query.Where(m => m.Title.Contains(movieFilter.Title));
        }

        if (movieFilter.GenreIds != null && movieFilter.GenreIds.Any())
        {
            query = query.Where(m => m.Genres.Any(g=> movieFilter.GenreIds.Contains(g.Id)));
        }

        if (movieFilter.MinAgeRating.HasValue)
        {
            query = query.Where(m => m.AgeRating >= movieFilter.MinAgeRating.Value);
        }

        query = query.ApplyOrderBy(
            movieFilter.OrderBy,
            movieFilter.SortDirection,
            MovieOrderByMap.Map);

        var skip = (movieFilter.PageNumber - 1) * movieFilter.PageSize;

        query = query.Skip(skip).Take(movieFilter.PageSize);

        try
        {
            return await query.ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database error while get filtered movies");
            throw;
        }
    }

    public async Task<int> CountFilteredAsync(MovieFilter movieFilter, CancellationToken cancellationToken)
    {
        var query = _dbContext.Movies.AsQueryable();
        
        if (!string.IsNullOrEmpty(movieFilter.Title))
        {
            query = query.Where(m => m.Title.Contains(movieFilter.Title));
        }
        
        
        if (movieFilter.GenreIds != null && movieFilter.GenreIds.Any())
        {
            query = query.Where(m => m.Genres.Any(g=> movieFilter.GenreIds.Contains(g.Id)));
        }
        
        if (movieFilter.MinAgeRating.HasValue)
        {
            query = query.Where(m => m.AgeRating >= movieFilter.MinAgeRating.Value);
        }

        try
        {
            return await query.CountAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database error while count filtered movies");
            throw;
        }
        
    }
}