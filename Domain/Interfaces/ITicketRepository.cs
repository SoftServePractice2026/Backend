using Domain.Entities;
using Domain.Filters;

namespace Domain.Interfaces
{
    public interface ITicketRepository
    {
        void CreateTicket(TicketEntity ticketEntity);
        void  DeleteTicket(TicketEntity ticketEntity);
        void  UpdateTicket(TicketEntity ticketEntity);
        Task<TicketEntity?> GetTicketByIdAsync(Guid ticketId, CancellationToken cancellationToken);
        Task<TicketEntity?> GetTicketBySeatAndSessionAsync(Guid seatId, Guid sessionId, CancellationToken cancellationToken);
        Task<List<TicketEntity>> GetFilteredTicketsAsync(TicketFilter ticketFilter, CancellationToken cancellationToken);
        Task<int> CountFilteredAsync(TicketFilter ticketFilter, CancellationToken cancellationToken);
    }
    
}