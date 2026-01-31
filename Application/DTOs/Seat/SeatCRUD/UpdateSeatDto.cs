using Domain.Entities.Enums;

namespace Application.DTOs.Seat.SeatCRUD;

public record UpdateSeatDto(
    Guid Id,
    int RowNumber,
    int SeatNumber,
    SeatTypeEnum SeatType,
    SeatStatusEnum SeatStatus
);