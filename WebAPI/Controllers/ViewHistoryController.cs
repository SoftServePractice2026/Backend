using Application.DTOs;
using Application.Services.ViewHistory;
using Domain.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

        public ViewHistoryController(IViewHistoryService viewHistoryService)
        {
            _viewHistoryService = viewHistoryService;
        }

        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ViewHistoryDetailsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Failure))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.UserPolicy)]
        [HttpPost]
        public async Task<IActionResult> PostViewHistory([FromBody] ViewHistoryCreateDto dto, CancellationToken cancellationToken)
        {
            var result = await _viewHistoryService.CreateViewHistoryAsync(dto, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }
            
            return CreatedAtAction(nameof(GetViewHistoryById), new { id = result.Value!.Id }, result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ViewHistoryDetailsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.UserPolicy)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutViewHistory(Guid id, ViewHistoryUpdateDto dto, CancellationToken cancellationToken)
        {
            var result = await _viewHistoryService.UpdateViewHistoryAsync(id, dto, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.UserPolicy)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteViewHistory(Guid id, CancellationToken cancellationToken)
        {
            var result = await _viewHistoryService.DeleteViewHistoryAsync(id, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }
            
            return Ok(result.Value);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ViewHistoryListItemDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.UserPolicy)]
        [HttpGet]
        public async Task<IActionResult> GetViewHistory([FromQuery] ViewHistoryFilterDto filterDto, CancellationToken cancellationToken)
        {
            var result = await _viewHistoryService.GetFilteredViewHistoryAsync(filterDto, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }
            
            Response.Headers.Append("X-Total-Count", result.Value.TotalCount.ToString());
            Response.Headers.Append("X-Page", filterDto.PageNumber.ToString());
            Response.Headers.Append("X-PageSize", filterDto.PageSize.ToString());
            
            return Ok(result.Value.Items);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ViewHistoryDetailsDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.UserPolicy)]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetViewHistoryById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _viewHistoryService.GetViewHistoryByIdAsync(id, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(result.Value);
        }
    }
}