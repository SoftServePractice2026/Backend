using Domain.Entities.Enums;

namespace Application.DTOs.Seat;

public record SeatListDto(
    Guid Id,
    int RowNumber,
    int SeatNumber,
    SeatTypeEnum SeatType,
    SeatStatusEnum SeatStatus
);