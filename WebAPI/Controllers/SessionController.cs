using Application.DTOs;
using Application.Services.Session;
using Domain.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using WebAPI.ResponseExtensions;

namespace WebAPI.Controllers;


[Route("api/v1/sessions")]
public class SessionController : BaseController
{
    private readonly ISessionService _sessionService;

    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }
    
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Failure))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.AdminPolicy)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SessionCreateDto dto, CancellationToken cancellationToken)
    {
        var result = await _sessionService.CreateSessionAsync(dto, cancellationToken);

        if (result.IsFailure)
            return result.Failure!.ToResponse();

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }
    
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SessionListItemDto>))]
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] SessionFilterDto filter, CancellationToken cancellationToken)
    {
        var result = await _sessionService.GetFilteredSessionsAsync(filter, cancellationToken);

        if (result.IsFailure)
            return result.Failure!.ToResponse();

        Response.Headers.Append("X-Total-Count", result.Value!.TotalCount.ToString());
        Response.Headers.Append("X-Page", filter.PageNumber.ToString());
        Response.Headers.Append("X-PageSize", filter.PageSize.ToString());

        return Ok(result.Value.Sessions);
    }
    
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SessionListItemDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sessionService.GetSessionByIdAsync(id, cancellationToken);

        if (result.IsFailure)
            return result.Failure!.ToResponse();

        return Ok(result.Value);
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SessionListItemDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.AdminPolicy)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] SessionUpdateDto dto, CancellationToken cancellationToken)
    {
        var result = await _sessionService.UpdateSessionAsync(id, dto, cancellationToken);

        if (result.IsFailure)
            return result.Failure!.ToResponse();

        return Ok(result.Value);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.AdminPolicy)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _sessionService.DeleteSessionAsync(id, cancellationToken);

        return NoContent();
    }
}