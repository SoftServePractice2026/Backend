using Application.DTOs.Seat;
using Application.DTOs.Seat.SeatCRUD;
using Shared;

namespace Application.Services.Seat;

public interface ISeatService
{
    Task<Result<SeatDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<SeatListDto>>> GetAllByHallIdAsync(Guid hallId, CancellationToken cancellationToken = default);
    Task<Result<SeatDto>> CreateAsync(CreateSeatDto dto, CancellationToken cancellationToken = default);
    Task<Result<SeatDto>> UpdateAsync(UpdateSeatDto dto, CancellationToken cancellationToken = default);
    Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}