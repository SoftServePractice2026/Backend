using Domain.Entities;

namespace Application.Interfaces;

public interface IBookingRepository
{
    Task<bool> SessionExistsAsync(Guid sessionId, CancellationToken cancellationToken);

    Task<List<string>> GetOccupiedSeatsAsync(Guid sessionId, CancellationToken cancellationToken);

    Task<bool> AreSeatsReservedAsync(Guid sessionId, List<string> seats, CancellationToken cancellationToken);

    Task AddTicketsAsync(List<TicketEntity> tickets, CancellationToken cancellationToken);
}