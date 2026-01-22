using Domain.Entities.Enums;

namespace Application.DTOs
{
    public record TicketDetailsDto(
        Guid Id,
        Guid UserId,
        Guid SeatId,
        Guid SessionId,
        Guid PaymentTransactionId,
        decimal Price,
        TicketStatusEnum TicketStatus);

    public record TicketCreateDto(
        Guid UserId,
        Guid SeatId,
        Guid SessionId,
        decimal Price,
        TicketStatusEnum TicketStatus);

    public record TicketUpdateDto(
        decimal Price,
        TicketStatusEnum TicketStatus);

    public record TicketListItemDto(
        Guid Id,
        Guid SessionId,
        decimal Price,
        TicketStatusEnum TicketStatus);
}
