using System.ComponentModel.DataAnnotations;
using Application.Dtos.Movie;
using Application.Services.Movie.MovieRepository;
using AutoMapper;
using CSharpFunctionalExtensions;
using Domain.Entities;
using Domain.Filters;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Extensions;

namespace Application.Services.Movie.MovieService;

public class MovieService : IMovieService
{
    
    private readonly IMovieRepository _movieRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<MovieService> _logger;


    public MovieService(
        IMovieRepository movieRepository, 
        IUnitOfWork unitOfWork,
        IMapper mapper, 
        ILogger<MovieService> logger)
    {
        _movieRepository = movieRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    
    
    public async Task<Result<MovieDetailsDto, Failure>> CreateMovieAsync(CreateMovieDto request, CancellationToken cancellationToken)
    {
        var existingMovie = await _movieRepository.GetMovieByNameAsync(request.Title, cancellationToken);

        if (existingMovie is not null)
        {
            return Failure.FromError(Error.Conflict("Movie.Exist", $"Movie with title: {request.Title} already exist"));
        }

        var movie = _mapper.Map<MovieEntity>(request);
        
        _movieRepository.AddMovie(movie);

        if (request.GenreIds != null && request.GenreIds.Any())
        {
            _movieRepository.AddGenresToMovie(movie, request.GenreIds);
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation($"Movie with id {movie.Id} was created.");
        
        var detailsDto = _mapper.Map<MovieDetailsDto>(movie);
        return Result.Success<MovieDetailsDto, Failure>(detailsDto);
    }

    public async Task<Result<MovieDetailsDto, Failure>> UpdateMovieAsync(UpdateMovieDto request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetMovieByIdAsync(request.Id, cancellationToken);

        if (movie is null)
        {
            _logger.LogWarning($"Movie with id {request.Id} was not found.");
            
            return Failure.FromError(Error.NotFound("MovieNotFound", "Movie not found"));
        }
        
        _mapper.Map(request, movie);
        
        _movieRepository.UpdateMovie(movie);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation($"Movie with id {movie.Id} was updated.");
        
        var detailsDto = _mapper.Map<MovieDetailsDto>(movie);
        
        return Result.Success<MovieDetailsDto, Failure>(detailsDto);
    }

    public async Task<Result<bool, Failure>> DeleteMovieAsync(DeleteMovieDto request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetMovieByIdAsync(request.Id, cancellationToken);

        if (movie == null)
        {
            _logger.LogWarning($"Movie with id {request.Id} was not found.");
            
            return Failure.FromError(Error.NotFound("MovieNotFound", "Movie not found"));
        }
        
        _movieRepository.DeleteMovie(movie);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation($"Movie with id {movie.Id} was deleted.");
        
        return Result.Success<bool, Failure>(true);

    }

    public async Task<Result<MovieDetailsDto, Failure>> GetMovieByIdAsync(GetMovieByIdDto request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetMovieByIdAsync(request.Id, cancellationToken);
        
        if (movie is null)
        {
            _logger.LogWarning($"Movie with id {request.Id} was not found.");
            
            return Failure.FromError(Error.NotFound("MovieNotFound", "Movie not found"));
        }

        var detailsDto = _mapper.Map<MovieDetailsDto>(movie);
        
        return Result.Success<MovieDetailsDto, Failure>(detailsDto);
    }

    public async Task<Result<(List<MovieListItemDto> Movies, int TotalCount), Failure>> GetFilteredMoviesAsync(
        MovieFilterDto movieFilterDto, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting filtered movies.");
        
        var filter = _mapper.Map<MovieFilter>(movieFilterDto);

        var movies = await _movieRepository.GetFilteredMoviesAsync(filter, cancellationToken);
        var totalCount = await _movieRepository.CountFilteredAsync(filter, cancellationToken);
        
        var moviesDto = _mapper.Map<List<MovieListItemDto>>(movies);
        
        
        return Result.Success<(List<MovieListItemDto>, int), Failure>((moviesDto, totalCount));
    }

    public async Task<Result<MovieDetailsDto, Failure>> AddGenresToMovieAsync(AddGenresToMovieDto request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Add genres to movie.");
        
        var movie = await _movieRepository.GetMovieByIdAsync(request.MovieId, cancellationToken);

        if (movie is null)
        {
            _logger.LogWarning($"Movie with id {request.MovieId} was not found.");
            return Failure.FromError(Error.NotFound("MovieNotFound", "Movie not found"));
        }
        
        _movieRepository.AddGenresToMovie(movie, request.GenreIds);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation($"Genres were added to movie with id {movie.Id}.");
        
        var detailsDto = _mapper.Map<MovieDetailsDto>(movie);
        
        return Result.Success<MovieDetailsDto, Failure>(detailsDto);
    }

    public async Task<Result<MovieDetailsDto, Failure>> AddActorsToMovieAsync(AddActorsToMovieDto request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Add actors to movie");
        
        var movie = await _movieRepository.GetMovieByIdAsync(request.MovieId, cancellationToken);

        if (movie is null)
        {
            _logger.LogWarning($"Movie with id {request.MovieId} was not found.");
            return Failure.FromError(Error.NotFound("MovieNotFound", "Movie not found"));
        }
        _movieRepository.AddActorsToMovie(movie, request.ActorIds);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        
        var updatedMovie = await _movieRepository.GetMovieByIdAsync(movie.Id, cancellationToken);
        
        _logger.LogInformation($"Actors were added to movie with id {movie.Id}.");
        
        var detailsDto = _mapper.Map<MovieDetailsDto>(updatedMovie);
        
        return Result.Success<MovieDetailsDto, Failure>(detailsDto);
    }

    public async Task<Result<bool, Failure>> DeleteGenresFromMovieAsync(DeleteGenresFromMovieDto request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Delete genres from movie.");
        
        var movie = await _movieRepository.GetMovieByIdAsync(request.MovieId, cancellationToken);

        if (movie is null)
        {
            _logger.LogWarning($"Movie with id {request.MovieId} was not found.");
            return Failure.FromError(Error.NotFound("MovieNotFound", "Movie not found"));
        }
        
        _movieRepository.DeleteGenresFromMovie(movie, request.GenreIds);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation($"Genres were deleted from movie with id {request.MovieId}.");
        
        
        return Result.Success<bool, Failure>(true);
    }

    public async Task<Result<bool, Failure>> DeleteActorsFromMovieAsync(DeleteActorsFromMovieDto request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Delete actors from movie.");
        
        var movie = await _movieRepository.GetMovieByIdAsync(request.MovieId, cancellationToken);
        
        if (movie is null)
        {
            _logger.LogWarning($"Movie with id {request.MovieId} was not found.");
            return Failure.FromError(Error.NotFound("MovieNotFound", "Movie not found"));
        }
        
        _movieRepository.DeleteActorsFromMovie(movie, request.ActorIds);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation($"Actors were deleted from movie with id {request.MovieId}.");
        
        return Result.Success<bool, Failure>(true);
    }
}