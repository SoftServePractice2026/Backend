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

        var movieEntity = MovieEntity.Create(
                poster: request.Poster,
                title: request.Title,
                description: request.Description,
                ageRating: request.AgeRating,
                language: request.Language,
                duration: request.Duration,
                start: request.RentalStartDate,
                end: request.RentalEndDate
            );

        if (movieEntity is null)
        {
            return Failure.FromError(Error.Validation("MovieCreationError", "Movie creation error"));
        }
        
        
        _movieRepository.AddMovie(movieEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation($"Movie with id {movieEntity.Id} was created.");
        
        var detailsDto = _mapper.Map<MovieDetailsDto>(movieEntity);
        
        return Result.Success<MovieDetailsDto, Failure>(detailsDto);
    }

    public async Task<Result<MovieDetailsDto, Failure>> UpdateMovieAsync(UpdateMovieDto request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetMovieByIdAsync(request.Id, cancellationToken);

        if (movie is null)
        {
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
}