using Domain.Entities;
using Domain.Entities.Extensions;
using Domain.Filters;
using Domain.Interfaces;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly CinemaDbContext _context;
        private readonly ILogger<TicketRepository> _logger;
        
        
        public TicketRepository(CinemaDbContext context, ILogger<TicketRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public void CreateTicket(TicketEntity ticketEntity) => _context.Tickets.Add(ticketEntity);
        public void DeleteTicket(TicketEntity ticketEntity) => _context.Tickets.Remove(ticketEntity);
        public void UpdateTicket(TicketEntity ticketEntity) => _context.Tickets.Update(ticketEntity);

        public async Task<TicketEntity?> GetTicketByIdAsync(Guid ticketId, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Tickets.FindAsync([ticketId], cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Database error while getting ticket by id: {TicketId}", ticketId);
                throw;
            }
        }

        public async Task<TicketEntity?> GetTicketBySeatAndSessionAsync(Guid seatId, Guid sessionId,
            CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Tickets
                    .FirstOrDefaultAsync(t => t.SeatId == seatId && t.SessionId == sessionId, cancellationToken);
            
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Database error while checking seat availability. SeatId: {SeatId}, SessionId: {SessionId}", seatId, sessionId);
                throw;

            }
        }

        public async Task<List<TicketEntity>> GetFilteredTicketsAsync(TicketFilter ticketFilter,
            CancellationToken cancellationToken)
        {
            var query = _context.Tickets.AsQueryable();
            
            if (ticketFilter.UserId.HasValue)
                query = query.Where(t => t.UserId == ticketFilter.UserId.Value);

            if (ticketFilter.TicketStatus.HasValue)
                query = query.Where(t => t.TicketStatus == ticketFilter.TicketStatus.Value);
            
            query = query.ApplyOrderBy(
                ticketFilter.OrderBy,
                ticketFilter.SortDirection,
                TicketOrederByMap.Map);
            
            var skip = (ticketFilter.PageNumber - 1) * ticketFilter.PageSize;
            query = query.Skip(skip).Take(ticketFilter.PageSize);
            
            try
            {
                return await query.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error while filtering tickets");
                throw;
            }
        }
        
        
        
        
        public async Task<int> CountFilteredAsync(TicketFilter ticketFilter, CancellationToken cancellationToken)
        {
            var query = _context.Tickets.AsQueryable();

            if (ticketFilter.UserId.HasValue)
                query = query.Where(t => t.UserId == ticketFilter.UserId.Value);

            if (ticketFilter.TicketStatus.HasValue)
                query = query.Where(t => t.TicketStatus == ticketFilter.TicketStatus.Value);

            try
            {
                return await query.CountAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error while counting filtered tickets");
                throw;
            }
        }
    }
}