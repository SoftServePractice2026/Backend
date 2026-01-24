using Application.DTOs;
using Application.Services.Hall;
using Microsoft.AspNetCore.Mvc;
using Shared;
using WebAPI.ResponseExtensions;

namespace WebAPI.Controllers
{
    [Route("api/hall")]
    public class HallController : BaseController
    {
        private readonly IHallService _hallService;

        public HallController(IHallService hallService)
        {
            _hallService = hallService;
        }

        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(HallDetailsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Failure))]
        [HttpPost]
        public async Task<IActionResult> PostHall([FromBody] HallCreateDto dto, CancellationToken cancellationToken)
        {
            var result = await _hallService.CreateHallAsync(dto, cancellationToken);

            return result.IsFailure
                ? result.Failure!.ToResponse()
                : CreatedAtAction(nameof(GetHallById), new { id = result.Value!.Id }, result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HallDetailsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Failure))]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutHall(Guid id, HallUpdateDto dto, CancellationToken cancellation)
        {
            var result = await _hallService.UpdateHallAsync(id, dto, cancellation);
            return result.IsFailure
                ? result.Failure!.ToResponse()
                : Ok(result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteHall(Guid id, CancellationToken cancellationToken)
        {
            var result = await _hallService.DeleteHallAsync(id, cancellationToken);
            return result.IsFailure
                ? result.Failure!.ToResponse()
                : Ok(result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HallDetailsDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetHallById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _hallService.GetHallByIdAsync(id, cancellationToken);
            return result.IsFailure
                ? result.Failure!.ToResponse()
                : Ok(result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HallDetailsDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [HttpGet("{name}")]
        public async Task<IActionResult> GetHallByName(string name, CancellationToken cancellationToken)
        {
            var result = await _hallService.GetHallByNameAsync(name, cancellationToken);
            return result.IsFailure
                ? result.Failure!.ToResponse()
                : Ok(result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HallDetailsDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [HttpGet]
        public async Task<IActionResult> GetAllHalls(CancellationToken cancellationToken)
        {
            var result = await _hallService.GetHallsAsync(cancellationToken);

            return result.IsFailure
                ? result.Failure!.ToResponse()
                : Ok(result.Value);
        }
    }
}
