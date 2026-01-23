using Application.Services.Movie.MovieRepository;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly CinemaDbContext _dbContext;

    public MovieRepository(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<MovieEntity?> AddMovieAsync(MovieEntity movieEntity)
    {
        if (movieEntity.Id == Guid.Empty) movieEntity.Id = Guid.NewGuid();
        
        await _dbContext.Movies.AddAsync(movieEntity);
        await _dbContext.SaveChangesAsync();
        
        return movieEntity;
        
    }

    public async Task<MovieEntity?> UpdateMovieAsync(MovieEntity movieEntity)
    {
        _dbContext.Movies.Update(movieEntity);
        await _dbContext.SaveChangesAsync();
        
        return movieEntity;
    }

    public async Task<bool> DeleteMovieAsync(MovieEntity movieEntity)
    {
        _dbContext.Movies.Remove(movieEntity);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<MovieEntity?> GetMovieByIdAsync(Guid movieId)
    {
        return await _dbContext.Movies
            .Include(m => m.Genres)
            .Include(m => m.ActorsInMovies)
            .FirstOrDefaultAsync(a => a.Id == movieId);
    }

    public async Task<MovieEntity?> GetMovieByNameAsync(string movieName)
    {
        return await _dbContext.Movies.FirstOrDefaultAsync(a => a.Title == movieName);
    }
}