using Application.DTOs;
using Application.DTOs.Actor;
using Application.Services.Actor;
using Application.Services.Hall;
using Microsoft.AspNetCore.Mvc;
using Shared;
using WebAPI.Mappers;
using WebAPI.ResponseExtensions;

namespace WebAPI.Controllers
{
    [Route("api/v1/actors")]
    public class ActorController : BaseController
    {
        private readonly IActorService _actorService;
        private readonly ILogger<ActorController> _logger;

        public ActorController(IActorService actorService, ILogger<ActorController> logger)
        {
            _actorService = actorService;
            _logger = logger;
        }

        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ActorDetailsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Failure))]
        [HttpPost]
        public async Task<IActionResult> PostActor([FromBody] ActorCreateDto dto, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request started: post hall");

            var result = await _actorService.CreateActorAsync(dto, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            _logger.LogInformation("Request ended: post actor");
            return CreatedAtAction(nameof(GetActorById), new { id = result.Value!.Id }, result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActorDetailsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Failure))]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutActor(Guid id, ActorUpdateDto dto, CancellationToken cancellation)
        {
            _logger.LogInformation("Request started: put actor");

            var result = await _actorService.UpdateActorAsync(id, dto, cancellation);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            _logger.LogInformation("Request ended: put actor");
            return Ok(result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(Failure))]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteActor(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request started: delete actor");

            var result = await _actorService.DeleteActorAsync(id, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            _logger.LogInformation("Request ended: delete actor");
            return Ok(result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ActorListItemDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [HttpGet]
        public async Task<IActionResult> GetActor([FromQuery] ActorFilterDto actorFilterDto, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request started: get filtered actor");

            var result = await _actorService.GetFilteredActorsAsync(actorFilterDto, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            Response.Headers.Append("X-Total-Count", result.Value.TotalCount.ToString());
            Response.Headers.Append("X-Page", actorFilterDto.Page.ToString());
            Response.Headers.Append("X-PageSize", actorFilterDto.PageSize.ToString());

            _logger.LogInformation("Request ended: get filtered actor");
            return Ok(result.Value.Actors);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActorDetailsDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetActorById(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request started: get actor by id");

            var result = await _actorService.GetActorByIdAsync(id, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            _logger.LogInformation("Request ended: get actor by id");
            return Ok(result.Value);
        }


    }
}
