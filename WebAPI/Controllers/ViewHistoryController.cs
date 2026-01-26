using Application.DTOs;
using Application.Services;
using Application.Services.ViewHistory;
using Microsoft.AspNetCore.Mvc;
using Shared;
using WebAPI.ResponseExtensions;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/view-histories")] 
    public class ViewHistoryController : BaseController
    {
        private readonly IViewHistoryService _viewHistoryService;
        private readonly ILogger<ViewHistoryController> _logger;

        public ViewHistoryController(IViewHistoryService viewHistoryService, ILogger<ViewHistoryController> logger)
        {
            _viewHistoryService = viewHistoryService;
            _logger = logger;
        }

        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ViewHistoryDetailsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Failure))]
        [HttpPost]
        public async Task<IActionResult> PostViewHistory([FromBody] ViewHistoryCreateDto dto, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request started: post view history");

            var result = await _viewHistoryService.CreateViewHistoryAsync(dto, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            _logger.LogInformation("Request ended: post view history");
            
            return CreatedAtAction(nameof(GetViewHistoryById), new { id = result.Value!.Id }, result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ViewHistoryDetailsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutViewHistory(Guid id, ViewHistoryUpdateDto dto, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request started: put view history");

            var result = await _viewHistoryService.UpdateViewHistoryAsync(id, dto, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            _logger.LogInformation("Request ended: put view history");
            return Ok(result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteViewHistory(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request started: delete view history");

            var result = await _viewHistoryService.DeleteViewHistoryAsync(id, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            _logger.LogInformation("Request ended: delete view history");
            return Ok(result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ViewHistoryListItemDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [HttpGet]
        public async Task<IActionResult> GetViewHistory([FromQuery] ViewHistoryFilterDto filterDto, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request started: get filtered view history");

            var result = await _viewHistoryService.GetFilteredViewHistoryAsync(filterDto, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }
            
            Response.Headers.Append("X-Total-Count", result.Value.TotalCount.ToString());
            Response.Headers.Append("X-Page", filterDto.Page.ToString());
            Response.Headers.Append("X-PageSize", filterDto.PageSize.ToString());

            _logger.LogInformation("Request ended: get filtered view history");
            return Ok(result.Value.Items);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ViewHistoryDetailsDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetViewHistoryById(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request started: get view history by id");

            var result = await _viewHistoryService.GetViewHistoryByIdAsync(id, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            _logger.LogInformation("Request ended: get view history by id");
            return Ok(result.Value);
        }
    }
}