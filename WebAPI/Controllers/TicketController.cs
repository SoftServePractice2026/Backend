using Application.DTOs;
using Application.Services.Hall;
using Application.Services.Ticket;
using Microsoft.AspNetCore.Mvc;
using Shared;
using WebAPI.ResponseExtensions;

namespace WebAPI.Controllers
{
    [Route("api/v1/tickets")]
    public class TicketController : BaseController
    {
        private readonly ITicketService _ticketService;
        private readonly ILogger<TicketController> _logger;

        public TicketController(ITicketService ticketService, ILogger<TicketController> logger)
        {
            _ticketService = ticketService;
            _logger = logger;
        }

        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TicketDetailsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Failure))]
        [HttpPost]
        public async Task<IActionResult> PostTicket([FromBody] TicketCreateDto dto, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request started: post ticket");

            var result = await _ticketService.CreateTicketAsync(dto, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            _logger.LogInformation("Request ended: post ticket");

            return CreatedAtAction(nameof(GetTicketById), new { id = result.Value!.Id }, result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TicketDetailsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [HttpPut("{id:guid}")]
        
        public async Task<IActionResult> PutTicket(Guid id, [FromBody] TicketUpdateDto dto, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request started: put ticket");

            var result = await _ticketService.UpdateTicketAsync(id, dto, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            _logger.LogInformation("Request ended: put ticket");
            return Ok(result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTicket(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request started: delete ticket");

            var result = await _ticketService.DeleteTicketAsync(id, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            _logger.LogInformation("Request ended: delete ticket");
            return Ok(result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TicketListItemDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [HttpGet]
        public async Task<IActionResult> GetTickets([FromQuery] TicketFilterDto ticketFilterDto, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request started: get filtered tickets");

            var result = await _ticketService.GetFilteredTicketsAsync(ticketFilterDto, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }
            
            Response.Headers.Append("X-Total-Count", result.Value.TotalCount.ToString());
            Response.Headers.Append("X-Page", ticketFilterDto.PageNumber.ToString());
            Response.Headers.Append("X-PageSize", ticketFilterDto.PageSize.ToString());

            _logger.LogInformation("Request ended: get filtered tickets");
            return Ok(result.Value.Tickets);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TicketDetailsDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTicketById(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request started: get ticket by id");

            var result = await _ticketService.GetTicketByIdAsync(id, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            _logger.LogInformation("Request ended: get ticket by id");
            return Ok(result.Value);
        }
    }
}