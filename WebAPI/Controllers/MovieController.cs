using Application.Dtos.Movie;
using Application.Services.Movie.MovieService;
using Domain.Filters;
using Microsoft.AspNetCore.Mvc;
using Shared;
using WebAPI.ResponseExtensions;

namespace WebAPI.Controllers;

[Route("api/v1/movies")]
public class MovieController : BaseController
{
    private readonly ILogger<MovieController> _logger;
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService, ILogger<MovieController> logger)
    {
        _movieService = movieService;
        _logger = logger;
    }

    
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MovieDetailsDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [HttpPost]
    public async Task<IActionResult> PostMovie(
        [FromBody] CreateMovieDto request,
        CancellationToken cancellationToken)
    {
        var result = await _movieService.CreateMovieAsync(request, cancellationToken);
        
        
        _logger.LogInformation("Request ended: post movie");
        
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }


    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetMovieByIdDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetMovieById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var request = new GetMovieByIdDto(id);
        
        var result = await _movieService.GetMovieByIdAsync(request, cancellationToken);
        
        _logger.LogInformation("Request ended: get movie by id");
        
        return result.IsFailure 
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }
    
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MovieDetailsDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Failure))]
    [HttpPut]
    public async Task<IActionResult> PutMovie(
        [FromBody] UpdateMovieDto request,
        CancellationToken cancellationToken)
    {
        var result = await _movieService.UpdateMovieAsync(request, cancellationToken);
        
        _logger.LogInformation("Request ended: put movie");
        
        return result.IsFailure 
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }

    
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMovie(
        Guid id,
        CancellationToken cancellationToken)
    {

        var request = new DeleteMovieDto(id);
        var result = await _movieService.DeleteMovieAsync(request, cancellationToken);
        
        
        _logger.LogInformation("Request ended: delete movie");
        
        return result.IsFailure 
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MovieListItemDto>))]

    public async Task<IActionResult> GetMovies([FromQuery] MovieFilterDto filter, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request started: get filtered movies");
        
        var result = await _movieService.GetFilteredMoviesAsync(filter, cancellationToken);

        if (result.IsFailure)
        {
            return  result.Error.ToResponse();
        }

        Response.Headers.Append("X-Total-Count", result.Value.TotalCount.ToString());
        Response.Headers.Append("X-Page", filter.Page.ToString());
        Response.Headers.Append("X-PageSize", filter.PageSize.ToString());

        _logger.LogInformation("Request ended: get filtered movies");
        return Ok(result.Value.Movies);
    }
    
}