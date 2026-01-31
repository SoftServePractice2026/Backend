using Domain.Entities;

namespace Domain.Interfaces;

public interface ISeatRepository
{
    void CreateSeat(SeatEntity seatEntity);
    void DeleteSeat(SeatEntity seatEntity);
    void UpdateSeat(SeatEntity seatEntity);
    
    Task<SeatEntity?>  GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<SeatEntity>> GetAllByHallIdAsync(Guid hallId, CancellationToken cancellationToken = default);
    Task<SeatEntity?> GetByPositionAsync(Guid hallId, int rowNumber, int seatNumber, CancellationToken cancellationToken = default);

    Task<bool> ExistsByPositionAsync(Guid hallId, int rowNumber, int seatNumber, CancellationToken cancellationToken = default);
    Task<bool> HasTicketsAsync(Guid seatId, CancellationToken cancellationToken = default);
}