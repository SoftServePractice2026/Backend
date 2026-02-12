using Application.Interfaces;
using Domain.Entities;
using Shared;
using Domain.Entities.Enums;


namespace Application.Services.Booking;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _repository;

    public BookingService(IBookingRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<string>>> GetOccupiedSeatsAsync(
        Guid sessionId,
        CancellationToken cancellationToken)
    {
        var sessionExists = await _repository.SessionExistsAsync(sessionId, cancellationToken);

        if (!sessionExists)
            return Result<List<string>>.Fail(
                Error.NotFound("Session.NotFound", "Session not found").ToFailure()
            );

        var seats = await _repository.GetOccupiedSeatsAsync(sessionId, cancellationToken);

        return Result<List<string>>.Success(seats);
    }

    public async Task<Result<bool>> ReserveSeatsAsync(
        Guid sessionId,
        List<string> seats,
        Guid userId,
        CancellationToken cancellationToken)
    {
        var sessionExists = await _repository.SessionExistsAsync(sessionId, cancellationToken);

        if (!sessionExists)
            return Result<bool>.Fail(
                Error.NotFound("Session.NotFound", "Session not found").ToFailure()
            );

        var alreadyReserved = await _repository.AreSeatsReservedAsync(
            sessionId,
            seats,
            cancellationToken);

        if (alreadyReserved)
            return Result<bool>.Fail(
                Error.Conflict("Seat.AlreadyReserved", "Some seats are already reserved").ToFailure()
            );

        var tickets = seats.Select(seatId => new TicketEntity
        {
            UserId = userId,
            SessionId = sessionId,
            SeatId = Guid.Parse(seatId),
            Price = 0, 
            TicketStatus = TicketStatusEnum.Reserved
        }).ToList();

        await _repository.AddTicketsAsync(tickets, cancellationToken);

        return Result<bool>.Success(true);
    }
}

