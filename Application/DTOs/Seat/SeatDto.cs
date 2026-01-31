using Domain.Entities.Enums;

namespace Application.DTOs.Seat;

public record SeatDto(
    Guid Id,
    Guid HallId,
    int RowNumber,
    int SeatNumber,
    SeatTypeEnum SeatType,
    SeatStatusEnum SeatStatus
);