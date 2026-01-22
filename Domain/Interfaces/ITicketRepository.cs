using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITicketRepository
    {
        Task CreateTicketAsync(TicketEntity ticketEntity);
        Task DeleteTicketAsync(TicketEntity ticketEntity);
        Task UpdateTicketAsync(TicketEntity ticketEntity);
        Task<TicketEntity?> GetTicketByIdAsync(Guid ticketId);
        Task<List<TicketEntity>> GetTicketEntitiesAsync();
        Task SaveChangesAsync();
    }
}