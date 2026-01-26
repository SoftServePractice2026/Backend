
using Domain.Entities.Enums;

namespace Application.DTOs
{
    public record SessionCreateDto(
        Guid MovieId,
        Guid HallId,
        DateTime StartTime,
        DateTime EndTime,
        SessionStatusEnum SessionStatus,
        decimal Price
    );
    
    
    public record SessionUpdateDto(
        DateTime? StartTime,
        DateTime? EndTime,
        SessionStatusEnum? SessionStatus,
        decimal? Price
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