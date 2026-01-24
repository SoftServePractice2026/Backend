using Application.DTOs;
using Application.Services.Session;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Mappers;

namespace WebAPI.Controllers;


[ApiController]
[Route("api/[controller]")]

public class SessionController : ControllerBase
{
    private readonly ISessionService _sessionService;
    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SessionDtos.SessionCreateDto dto)
    {
        var result = await _sessionService.CreateSessionAsync(dto);
        return result.IsFailure
            ? FailureMapper.ToHttp(result.Failure!)
            : CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }
    
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _sessionService.GetSessionAllAsync();

        return result.IsFailure
            ? FailureMapper.ToHttp(result.Failure!)
            : Ok(result.Value);
    }
    
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _sessionService.GetSessionByIdAsync(id);

        return result.IsFailure
            ? FailureMapper.ToHttp(result.Failure!)
            : Ok(result.Value);
    }

    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAll(Guid id, [FromBody] SessionDtos.SessionUpdateAllDto dto)
    {
        var result = await _sessionService.UpdateSessionAllAsync(id, dto);

        return result.IsFailure
            ? FailureMapper.ToHttp(result.Failure!)
            : Ok(result.Value);
    }


    
    [HttpPatch("{id:guid}/time")]
    public async Task<IActionResult> UpdateTime(Guid id, [FromBody] SessionDtos.SessionUpdateTimeDto dto)
    {
        var result = await _sessionService.UpdateSessionTimeAsync(id, dto);

        return result.IsFailure
            ? FailureMapper.ToHttp(result.Failure!)
            : Ok(result.Value);
    }


    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] SessionDtos.SessionUpdateStatusDto dto)
    {
        var result = await _sessionService.UpdateSessionStatusAsync(id, dto);
        return result.IsFailure
            ? FailureMapper.ToHttp(result.Failure!)
            : Ok(result.Value);
    }
    

    [HttpPatch("{id:guid}/price")]
    public async Task<IActionResult> UpdatePrice(Guid id, [FromBody] SessionDtos.SessionUpdatePriceDto dto)
    {
        var result = await _sessionService.UpdateSessionPriceAsync(id, dto);
        return result.IsFailure
            ? FailureMapper.ToHttp(result.Failure!)
            : Ok(result.Value);
    }

    
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _sessionService.DeleteSessionAsync(id);

        return result.IsFailure
            ? FailureMapper.ToHttp(result.Failure!)
            : NoContent();
    }
}