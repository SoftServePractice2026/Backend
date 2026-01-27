using Domain.Entities.Enums;
using Application.DTOs.Common;
using Application.Interfaces;
using Shared.Common;

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

    public record TicketFilterDto(
        Guid? UserId,
        TicketStatusEnum? TicketStatus,
        string? OrderBy,
        SortDirection SortDirection = SortDirection.Ascending) : PaginationBaseDto, ISortable;
}
