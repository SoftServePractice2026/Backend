using System.ComponentModel.DataAnnotations;
using Application.Dtos.Movie;
using Application.Services.Movie.MovieRepository;
using AutoMapper;
using CSharpFunctionalExtensions;
using Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Extensions;

namespace Application.Services.Movie.MovieService;

public class MovieService : IMovieService
{
    
    private readonly IMovieRepository _movieRepository;
    private readonly IValidator<CreateMovieDto> _createMovieValidator;
    private readonly IValidator<UpdateMovieDto> _updateMovieValidator;
    private readonly IValidator<DeleteMovieDto> _deleteMovieValidator;
    private readonly IValidator<GetMovieByIdDto> _getMovieByIdValidator;
    private readonly IMapper _mapper;
    private readonly ILogger<MovieService> _logger;


    public MovieService(
        IMovieRepository movieRepository, 
        IValidator<CreateMovieDto> createMovieValidator,
        IValidator<UpdateMovieDto> updateMovieValidator, 
        IValidator<DeleteMovieDto> deleteMovieValidator, 
        IValidator<GetMovieByIdDto> getMovieByIdValidator, 
        IMapper mapper, 
        ILogger<MovieService> logger)
    {
        _movieRepository = movieRepository;
        _createMovieValidator = createMovieValidator;
        _updateMovieValidator = updateMovieValidator;
        _deleteMovieValidator = deleteMovieValidator;
        _getMovieByIdValidator = getMovieByIdValidator;
        _mapper = mapper;
        _logger = logger;
    }
    
    
    public async Task<Result<MovieDetailsDto, Failure>> CreateMovieAsync(CreateMovieDto request, CancellationToken cancellationToken)
    {
        var validationResult = await _createMovieValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToErrors();
        }


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
        
        var result = await _movieRepository.AddMovieAsync(movieEntity);
        
        _logger.LogInformation($"Movie with id {movieEntity.Id} was created.");
        
        var detailsDto = _mapper.Map<MovieDetailsDto>(result);
        
        return Result.Success<MovieDetailsDto, Failure>(detailsDto);
    }

    public async Task<Result<MovieDetailsDto, Failure>> UpdateMovieAsync(UpdateMovieDto request, CancellationToken cancellationToken)
    {
        var validationResult = await _updateMovieValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToErrors();
        }
        
        var movie = await _movieRepository.GetMovieByIdAsync(request.Id);

        if (movie is null)
        {
            return Failure.FromError(Error.Validation("MovieNotFound", "Movie not found"));
        }
        
        var result = await _movieRepository.UpdateMovieAsync(movie);
        
        _logger.LogInformation($"Movie with id {movie.Id} was updated.");
        
        var detailsDto = _mapper.Map<MovieDetailsDto>(result);
        
        return Result.Success<MovieDetailsDto, Failure>(detailsDto);
    }

    public async Task<Result<bool, Failure>> DeleteMovieAsync(DeleteMovieDto request, CancellationToken cancellationToken)
    {
        var validationResult = await _deleteMovieValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToErrors();
        }
        
        var movie = await _movieRepository.GetMovieByIdAsync(request.Id);

        var result = await _movieRepository.DeleteMovieAsync(movie);
        
        _logger.LogInformation($"Movie with id {movie.Id} was deleted.");
        
        var detailsDto = _mapper.Map<MovieDetailsDto>(result);
        
        return Result.Success<bool, Failure>(true);

    }

    public async Task<Result<MovieDetailsDto, Failure>> GetMovieByIdAsync(GetMovieByIdDto request, CancellationToken cancellationToken)
    {
        var validationResult = await _getMovieByIdValidator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            return validationResult.ToErrors();
        }
        
        var movie = await _movieRepository.GetMovieByIdAsync(request.Id);
        
        if (movie is null)
        {
            return Failure.FromError(Error.Validation("MovieNotFound", "Movie not found"));
        }

        var detailsDto = _mapper.Map<MovieDetailsDto>(movie);
        
        return Result.Success<MovieDetailsDto, Failure>(detailsDto);
    }
}