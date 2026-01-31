using Application.DTOs.Genre;
using Application.Services.Genre;
using Microsoft.AspNetCore.Mvc;
using Shared;
using WebAPI.ResponseExtensions;

namespace WebAPI.Controllers
{
    [Route("api/v1/genres")]
    public class GenreController : BaseController
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenreDetailsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Failure))]
        [HttpPost]
        public async Task<IActionResult> PostGenre([FromBody] GenreCreateDto dto, CancellationToken cancellationToken)
        {
            var result = await _genreService.CreateGenreAsync(dto, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return CreatedAtAction(nameof(GetGenreById), new { id = result.Value!.Id }, result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenreDetailsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Failure))]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutGenre(Guid id, GenreUpdateDto dto, CancellationToken cancellation)
        {
            var result = await _genreService.UpdateGenreAsync(id, dto, cancellation);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(Failure))]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteGenre(Guid id, CancellationToken cancellationToken)
        {
            var result = await _genreService.DeleteGenreAsync(id, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GenreListItemDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [HttpGet]
        public async Task<IActionResult> GetGenre([FromQuery] GenreFilterDto filter, CancellationToken cancellationToken)
        {
            var result = await _genreService.GetGenreAllAsync(filter, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenreDetailsDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetGenreById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _genreService.GetGenreByIdAsync(id, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenreDetailsDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [HttpGet("{name}")]
        public async Task<IActionResult> GetGenreByName(string name, CancellationToken cancellationToken)
        {
            var result = await _genreService.GetGenreByNameAsync(name, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }
            
            return Ok(result.Value);
        }
    }
}
