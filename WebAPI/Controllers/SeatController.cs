using Application.DTOs.Seat;
using Application.DTOs.Seat.SeatCRUD;
using Application.Services.Seat;
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

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SeatDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _seatService.GetByIdAsync(id, cancellationToken);

        return result.IsFailure
            ? FailureMapper.ToHttp(result.Failure!)
            : Ok(result.Value);
    }
    
    [HttpGet("hall/{hallId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<SeatListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllByHallId(Guid hallId, CancellationToken cancellationToken)
    {
        var result = await _seatService.GetAllByHallIdAsync(hallId, cancellationToken);

        return result.IsFailure
            ? FailureMapper.ToHttp(result.Failure!)
            : Ok(result.Value);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(SeatDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateSeatDto dto, CancellationToken cancellationToken)
    {
        var result = await _seatService.CreateAsync(dto, cancellationToken);

        if (result.IsFailure)
        {
            return FailureMapper.ToHttp(result.Failure!);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
    }
    
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(SeatDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
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
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
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