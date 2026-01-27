using Application.DTOs;
using Application.Services.Session;
using Microsoft.AspNetCore.Mvc;
using Shared;
using WebAPI.ResponseExtensions;

namespace WebAPI.Controllers;


[Route("api/v1/sessions")]
public class SessionController : BaseController
{
    private readonly ISessionService _sessionService;
    private readonly ILogger<SessionController> _logger;

    public SessionController(ISessionService sessionService, ILogger<SessionController> logger)
    {
        _sessionService = sessionService;
        _logger = logger;
    }
    
    
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Failure))]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SessionCreateDto dto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request started: create session");
        var result = await _sessionService.CreateSessionAsync(dto, cancellationToken);

        if (result.IsFailure)
            return result.Failure!.ToResponse();

        _logger.LogInformation("Request ended: create session");
        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }
    
    
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SessionListItemDto>))]
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] SessionFilterDto filter, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request started: get filtered sessions");
        var result = await _sessionService.GetFilteredSessionsAsync(filter, cancellationToken);

        if (result.IsFailure)
            return result.Failure!.ToResponse();

        Response.Headers.Append("X-Total-Count", result.Value.TotalCount.ToString());
        Response.Headers.Append("X-Page", filter.PageNumber.ToString());
        Response.Headers.Append("X-PageSize", filter.PageSize.ToString());

        _logger.LogInformation("Request ended: get filtered sessions");
        return Ok(result.Value.Sessions);
    }
    
    
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SessionListItemDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request started: get session by id. SessionId={SessionId}", id);
        var result = await _sessionService.GetSessionByIdAsync(id, cancellationToken);

        if (result.IsFailure)
            return result.Failure!.ToResponse();

        _logger.LogInformation("Request ended: get session by id. SessionId={SessionId}", id);
        return Ok(result.Value);
    }

    
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SessionListItemDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Failure))]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] SessionUpdateDto dto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request started: update session. SessionId={SessionId}", id);
        var result = await _sessionService.UpdateSessionAsync(id, dto, cancellationToken);

        if (result.IsFailure)
            return result.Failure!.ToResponse();

        _logger.LogInformation("Request ended: update session. SessionId={SessionId}", id);
        return Ok(result.Value);
    }

    
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request started: delete session. SessionId={SessionId}", id);
        await _sessionService.DeleteSessionAsync(id, cancellationToken);

        _logger.LogInformation("Request ended: delete session. SessionId={SessionId}", id);
        return NoContent();
    }

}