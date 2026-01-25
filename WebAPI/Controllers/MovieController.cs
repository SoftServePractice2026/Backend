using Application.Dtos.Movie;
using Application.Services.Movie.MovieService;
using Microsoft.AspNetCore.Mvc;
using WebAPI.ResponseExtensions;

namespace WebAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateMovie(
        [FromBody] CreateMovieDto request,
        CancellationToken cancellationToken)
    {
        var result = await _movieService.CreateMovieAsync(request, cancellationToken);
        
        return result.IsFailure ? result.Error.ToResponse() : Ok(result.Value);
    }


    [HttpGet]
    public async Task<IActionResult> GetMovie(
        [FromQuery] GetMovieByIdDto request,
        CancellationToken cancellationToken)
    {
        var result = await _movieService.GetMovieByIdAsync(request, cancellationToken);
        
        return result.IsFailure 
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }


    [HttpPut]
    public async Task<IActionResult> UpdateMovie(
        [FromBody] UpdateMovieDto request,
        CancellationToken cancellationToken)
    {
        var result = await _movieService.UpdateMovieAsync(request, cancellationToken);
        
        return result.IsFailure 
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }

    [HttpDelete]
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