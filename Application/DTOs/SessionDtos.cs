namespace Application.DTOs;
using Domain.Entities.Enums;

public class SessionDtos
{
    public record SessionCreateDto(
        Guid MovieId,
        Guid HallId,
        DateTime StartTime,
        DateTime EndTime,
        SessionStatusEnum SessionStatus,
        decimal Price
    );
    
    
    public record SessionUpdateAllDto(
        DateTime StartTime,
        DateTime EndTime,
        SessionStatusEnum SessionStatus,
        decimal Price
    );
    
    
    public record SessionUpdateTimeDto(
        DateTime StartTime,
        DateTime EndTime
    );

    
    public record SessionUpdateStatusDto(
        SessionStatusEnum SessionStatus
    );

    
    public record SessionUpdatePriceDto(
        decimal Price
    );
    
    
    public record SessionCardDto(
        Guid Id,
        string MovieTitle,
        Guid MovieId,
        string HallName,
        Guid HallId,
        DateTime StartTime,
        SessionStatusEnum SessionStatus,
        decimal Price,
        IReadOnlyList<SeatInSessionDto> Seats
    );
    
    
    public record SeatInSessionDto(
        Guid SeatId,
        int RowNumber,
        int SeatNumber,
        TicketStatusEnum TicketStatus
    );
    
}