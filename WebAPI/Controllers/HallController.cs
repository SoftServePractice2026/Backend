using Application.DTOs;
using Application.Services.Hall;
using Microsoft.AspNetCore.Mvc;
using Shared;
using WebAPI.ResponseExtensions;

namespace WebAPI.Controllers
{
    [Route("api/v1/halls")]
    public class HallController : BaseController
    {
        private readonly IHallService _hallService;
        public HallController(IHallService hallService, ILogger<HallController> logger)
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

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }
            
            return CreatedAtAction(nameof(PostHall), new { id = result.Value!.Id }, result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HallDetailsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Failure))]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutHall(Guid id, HallUpdateDto dto, CancellationToken cancellation)
        {
            var result = await _hallService.UpdateHallAsync(id, dto, cancellation);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteHall(Guid id, CancellationToken cancellationToken)
        {
            var result = await _hallService.DeleteHallAsync(id, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<HallListItemDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [HttpGet]
        public async Task<IActionResult> GetHall([FromQuery] HallFilterDto hallFilterDto, CancellationToken cancellationToken)
        {
            var result = await _hallService.GetFilteredHallsAsync(hallFilterDto, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            Response.Headers.Append("X-Total-Count", result.Value.TotalCount.ToString());
            Response.Headers.Append("X-Page", hallFilterDto.PageNumber.ToString());
            Response.Headers.Append("X-PageSize", hallFilterDto.PageSize.ToString());

            return Ok(result.Value.Halls);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HallDetailsDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetHallById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _hallService.GetHallByIdAsync(id, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }
            return Ok(result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HallDetailsDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [HttpGet("{name}")]
        public async Task<IActionResult> GetHallByName(string name, CancellationToken cancellationToken)
        {
            var result = await _hallService.GetHallByNameAsync(name, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }
            return Ok(result.Value);
        }
    }
}
