using Domain.Entities.Enums;

namespace Application.DTOs.Seat.SeatCRUD;

public record CreateSeatDto(
    Guid HallId,
    int RowNumber,
    int SeatNumber,
    SeatTypeEnum SeatType,
    SeatStatusEnum SeatStatus = SeatStatusEnum.Normal
);