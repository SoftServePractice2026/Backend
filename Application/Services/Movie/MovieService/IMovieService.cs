using Application.Dtos.Movie;
using CSharpFunctionalExtensions;
using Shared;

namespace Application.Services.Movie.MovieService;

public interface IMovieService
{
    Task<Result<MovieDetailsDto, Failure>> CreateMovieAsync(CreateMovieDto request, CancellationToken cancellationToken);
    
    Task<Result<MovieDetailsDto, Failure>> UpdateMovieAsync(UpdateMovieDto request, CancellationToken cancellationToken);
    
    Task<Result<bool, Failure>> DeleteMovieAsync(DeleteMovieDto request, CancellationToken cancellationToken);
    
    Task<Result<MovieDetailsDto, Failure>> GetMovieByIdAsync(GetMovieByIdDto request, CancellationToken cancellationToken);

    Task<Result<(List<MovieListItemDto> Movies, int TotalCount), Failure>> GetFilteredMoviesAsync(
        MovieFilterDto movieFilterDto, 
        CancellationToken cancellationToken);






    Task<Result<MovieDetailsDto, Failure>> AddGenresToMovieAsync(AddGenresToMovieDto request, CancellationToken cancellationToken);

    Task<Result<MovieDetailsDto, Failure>> AddActorsToMovieAsync(AddActorsToMovieDto request, CancellationToken cancellationToken);
    
    
    Task<Result<bool, Failure>> DeleteGenresFromMovieAsync(DeleteGenresFromMovieDto request, CancellationToken cancellationToken);
    
    Task<Result<bool, Failure>> DeleteActorsFromMovieAsync(DeleteActorsFromMovieDto request, CancellationToken cancellationToken);
    
    

}