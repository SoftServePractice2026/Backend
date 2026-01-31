using Application.DTOs;
using Shared;

namespace Application.Services.Ticket;

public interface ITicketService
{
    Task<Result<TicketDetailsDto>> CreateTicketAsync(TicketCreateDto dto, CancellationToken cancellationToken);
    
    Task<Result<TicketDetailsDto>> UpdateTicketAsync(Guid targetId, TicketUpdateDto dto, CancellationToken cancellationToken);
    
    Task<Result<bool>> DeleteTicketAsync(Guid id, CancellationToken cancellationToken);
    
    Task<Result<TicketDetailsDto>> GetTicketByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<Result<(List<TicketListItemDto> Tickets, int TotalCount)>> GetFilteredTicketsAsync(TicketFilterDto ticketFilterDto, CancellationToken cancellationToken);
}