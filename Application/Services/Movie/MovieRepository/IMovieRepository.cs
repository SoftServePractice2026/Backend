using Domain.Entities;

namespace Application.Services.Movie.MovieRepository;

public interface IMovieRepository
{
    Task<MovieEntity?> AddMovieAsync(MovieEntity movieEntity);
    
    Task<MovieEntity?> UpdateMovieAsync(MovieEntity movieEntity);
    
    Task<bool> DeleteMovieAsync(MovieEntity movieEntity);
    
    Task<MovieEntity?> GetMovieByIdAsync(Guid movieId);
    
    Task<MovieEntity?> GetMovieByNameAsync(string movieName);   
}