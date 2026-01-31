using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SeatRepository : ISeatRepository
{
    private readonly CinemaDbContext _dbContext;

    public SeatRepository(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void CreateSeat(SeatEntity seatEntity) => _dbContext.Seats.Add(seatEntity);
    public void DeleteSeat(SeatEntity seatEntity) => _dbContext.Seats.Remove(seatEntity);
    public void UpdateSeat(SeatEntity seatEntity) => _dbContext.Seats.Update(seatEntity);
    
    public async Task<SeatEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Seats.FindAsync([id], cancellationToken);
    }
    
    public async Task<IEnumerable<SeatEntity>> GetAllByHallIdAsync(Guid hallId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Seats
            .Where(s => s.HallId == hallId)
            .OrderBy(s => s.RowNumber)
            .ThenBy(s => s.SeatNumber)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<SeatEntity?> GetByPositionAsync(Guid hallId, int rowNumber, int seatNumber, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Seats
            .FirstOrDefaultAsync(s => 
                    s.HallId == hallId && 
                    s.RowNumber == rowNumber && 
                    s.SeatNumber == seatNumber, 
                cancellationToken);
    }

    public async Task<bool> ExistsByPositionAsync(Guid hallId, int rowNumber, int seatNumber, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Seats
            .AnyAsync(s => 
                    s.HallId == hallId && 
                    s.RowNumber == rowNumber && 
                    s.SeatNumber == seatNumber, 
                cancellationToken);
    }

    public async Task<bool> HasTicketsAsync(Guid seatId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Seats
            .Where(s => s.Id == seatId)
            .SelectMany(s => s.Tickets)
            .AnyAsync(cancellationToken);
    }
}