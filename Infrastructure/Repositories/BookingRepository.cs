using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly CinemaDbContext _dbContext;

    public BookingRepository(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> SessionExistsAsync(Guid sessionId, CancellationToken cancellationToken)
    {
        return await _dbContext.Sessions
            .AnyAsync(s => s.Id == sessionId, cancellationToken);
    }

    public async Task<List<string>> GetOccupiedSeatsAsync(Guid sessionId, CancellationToken cancellationToken)
    {
        return await _dbContext.Tickets
            .Where(t => t.SessionId == sessionId)
            .Select(t => t.SeatId.ToString()) 
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> AreSeatsReservedAsync(Guid sessionId, List<string> seats, CancellationToken cancellationToken)
    {
        var seatGuids = seats.Select(Guid.Parse).ToList();

        return await _dbContext.Tickets
            .AnyAsync(t => t.SessionId == sessionId && seatGuids.Contains(t.SeatId),
                cancellationToken);
    }

    public async Task AddTicketsAsync(List<TicketEntity> tickets, CancellationToken cancellationToken)
    {
        await _dbContext.Tickets.AddRangeAsync(tickets, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}