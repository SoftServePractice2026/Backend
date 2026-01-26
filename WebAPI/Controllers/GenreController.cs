using Application.DTOs;
using Application.DTOs.Genre;
using Application.Services.Genre;
using Application.Services.Hall;
using Microsoft.AspNetCore.Mvc;
using Shared;
using WebAPI.Mappers;
using WebAPI.ResponseExtensions;

namespace WebAPI.Controllers
{
    [Route("api/v1/genres")]
    public class GenreController : BaseController
    {
        private readonly IGenreService _genreService;
        private readonly ILogger<GenreController> _logger;

        public GenreController(IGenreService genreService, ILogger<GenreController> logger)
        {
            _genreService = genreService;
            _logger = logger;
        }

        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenreDetailsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Failure))]
        [HttpPost]
        public async Task<IActionResult> PostGenre([FromBody] GenreCreateDto dto, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request started: post genre");

            var result = await _genreService.CreateGenreAsync(dto, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            _logger.LogInformation("Request ended: post genre");
            return CreatedAtAction(nameof(GetGenreById), new { id = result.Value!.Id }, result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenreDetailsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Failure))]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutGenre(Guid id, GenreUpdateDto dto, CancellationToken cancellation)
        {
            _logger.LogInformation("Request started: put genre");

            var result = await _genreService.UpdateGenreAsync(id, dto, cancellation);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            _logger.LogInformation("Request ended: put genre");
            return Ok(result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(Failure))]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteGenre(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request started: delete genre");

            var result = await _genreService.DeleteGenreAsync(id, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            _logger.LogInformation("Request ended: delete genre");
            return Ok(result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GenreListItemDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [HttpGet]
        public async Task<IActionResult> GetGenre(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request started: get all genres");

            var result = await _genreService.GetGenreAllAsync(cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            _logger.LogInformation("Request ended: get all genres success");

            return Ok(result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenreDetailsDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetGenreById(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request started: get genre by id");

            var result = await _genreService.GetGenreByIdAsync(id, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            _logger.LogInformation("Request ended: get genre by id");
            return Ok(result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenreDetailsDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [HttpGet("{name}")]
        public async Task<IActionResult> GetGenreByName(string name, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request started: get genre by name");

            var result = await _genreService.GetGenreByNameAsync(name, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            _logger.LogInformation("Request ended: get genre by name");
            return Ok(result.Value);
        }
    }
}
