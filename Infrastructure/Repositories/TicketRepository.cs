using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly CinemaDbContext _context;
        public TicketRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task CreateTicketAsync(TicketEntity ticketEntity)
        {
            await _context.Tickets.AddAsync(ticketEntity);
        }

        public Task DeleteTicketAsync(TicketEntity ticketEntity)
        {
            _context.Tickets.Remove(ticketEntity);
            return Task.CompletedTask;
        }

        public Task<TicketEntity?> GetTicketByIdAsync(Guid ticketId) => _context.Tickets.FirstOrDefaultAsync(x => x.Id == ticketId);
        public Task<List<TicketEntity>> GetTicketEntitiesAsync() => _context.Tickets.ToListAsync();

        public Task UpdateTicketAsync(TicketEntity ticketEntity)
        {
            _context.Tickets.Update(ticketEntity);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}