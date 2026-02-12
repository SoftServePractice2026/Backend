using Application.DTOs.Booking;
using Application.Services.Booking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using WebAPI.ResponseExtensions;

namespace WebAPI.Controllers;

[Route("api/v1/bookings")]
public class BookingController : BaseController
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet("occupied-seats/{sessionId:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetOccupiedSeats(Guid sessionId, CancellationToken cancellationToken)
    {
        var result = await _bookingService.GetOccupiedSeatsAsync(sessionId, cancellationToken);

        if (result.IsFailure)
            return result.Failure!.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("reserve")]
    [Authorize]
    public async Task<IActionResult> ReserveSeats([FromBody] BookingCreateDto dto, CancellationToken cancellationToken)
    {
        var userId = GetUserId(); // якщо у тебе є метод в BaseController

        var result = await _bookingService.ReserveSeatsAsync(dto.ShowtimeId, dto.Seats, userId, cancellationToken);

        if (result.IsFailure)
            return result.Failure!.ToResponse();

        return Ok(result.Value);
    }
}