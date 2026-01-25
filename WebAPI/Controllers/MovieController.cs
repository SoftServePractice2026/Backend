using Application.Dtos.Movie;
using Application.Services.Movie.MovieService;
using Microsoft.AspNetCore.Mvc;
using WebAPI.ResponseExtensions;

namespace WebAPI.Controllers;

[Route("api/v1/movies")]
public class MovieController : BaseController
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateMovie(
        [FromBody] CreateMovieDto request,
        CancellationToken cancellationToken)
    {
        var result = await _movieService.CreateMovieAsync(request, cancellationToken);
        
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }


    [HttpGet("get")]
    public async Task<IActionResult> GetMovie(
        [FromQuery] GetMovieByIdDto request,
        CancellationToken cancellationToken)
    {
        var result = await _movieService.GetMovieByIdAsync(request, cancellationToken);
        
        return result.IsFailure 
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }


    [HttpPost("update")]
    public async Task<IActionResult> UpdateMovie(
        [FromBody] UpdateMovieDto request,
        CancellationToken cancellationToken)
    {
        var result = await _movieService.UpdateMovieAsync(request, cancellationToken);
        
        return result.IsFailure 
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteMovie(
        [FromBody] DeleteMovieDto request,
        CancellationToken cancellationToken)
    {
        var result = await _movieService.DeleteMovieAsync(request, cancellationToken);
        
        return result.IsFailure 
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }
    
}