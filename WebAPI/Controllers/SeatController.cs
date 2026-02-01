using Application.DTOs.Seat;
using Application.DTOs.Seat.SeatCRUD;
using Application.Services.Seat;
using Domain.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Mappers;

namespace WebAPI.Controllers;

[Route("api/v1/seat")]
public class SeatController : BaseController
{
    private readonly ISeatService _seatService;

    public SeatController(ISeatService seatService)
    {
        _seatService = seatService;
    }

    [ProducesResponseType(typeof(SeatDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _seatService.GetByIdAsync(id, cancellationToken);

        return result.IsFailure
            ? FailureMapper.ToHttp(result.Failure!)
            : Ok(result.Value);
    }
    
    [ProducesResponseType(typeof(IEnumerable<SeatListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    [HttpGet("hall/{hallId:guid}")]
    public async Task<IActionResult> GetAllByHallId(Guid hallId, CancellationToken cancellationToken)
    {
        var result = await _seatService.GetAllByHallIdAsync(hallId, cancellationToken);

        return result.IsFailure
            ? FailureMapper.ToHttp(result.Failure!)
            : Ok(result.Value);
    }
    
    [ProducesResponseType(typeof(SeatDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.AdminPolicy)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSeatDto dto, CancellationToken cancellationToken)
    {
        var result = await _seatService.CreateAsync(dto, cancellationToken);

        if (result.IsFailure)
        {
            return FailureMapper.ToHttp(result.Failure!);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
    }
    
    [ProducesResponseType(typeof(SeatDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.AdminPolicy)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSeatDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.Id)
        {
            return BadRequest(new { Error = "ID in URL does not match ID in body" });
        }

        var result = await _seatService.UpdateAsync(dto, cancellationToken);

        if (result.IsFailure)
        {
            FailureMapper.ToHttp(result.Failure!);
        }

        return Ok(result.Value);
    }
    
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.AdminPolicy)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _seatService.DeleteAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            FailureMapper.ToHttp(result.Failure!);
        }

        return NoContent();
    }
}