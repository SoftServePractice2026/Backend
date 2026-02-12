using Shared;

namespace Application.Services.Booking;

public interface IBookingService
{
    Task<Result<List<string>>> GetOccupiedSeatsAsync(Guid sessionId, CancellationToken cancellationToken);
    Task<Result<bool>> ReserveSeatsAsync(Guid sessionId, List<string> seats, Guid userId, CancellationToken cancellationToken);
}