using Domain.Entities;
using Domain.Filters;

namespace Application.Services.Movie.MovieRepository;

public interface IMovieRepository
{
    void AddMovie(MovieEntity movieEntity);
    
    void UpdateMovie(MovieEntity movieEntity);
    
    void DeleteMovie(MovieEntity movieEntity);
    
    Task<MovieEntity?> GetMovieByIdAsync(Guid movieId, CancellationToken cancellationToken);
    Task<MovieEntity?> GetMovieByNameAsync(string movieName, CancellationToken cancellationToken);

    Task<List<MovieEntity>> GetFilteredMoviesAsync(MovieFilter movieFilter, CancellationToken cancellationToken);
    
    Task<int> CountFilteredAsync(MovieFilter movieFilter, CancellationToken cancellationToken);
    
    
}