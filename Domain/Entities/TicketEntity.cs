using Domain.Entities.Enums;
using Domain.Entities.Interfaces;
using Domain.Primitives;

namespace Domain.Entities
{
    public class TicketEntity : AuditableEntity
    {
        public Guid UserId { get; set; }
        public IUser ApplicationUser { get; set; } = null!;

        public Guid SeatId { get; set; }
        public SeatEntity Seat { get; set; } = null!;

        public Guid SessionId { get; set; }
        public SessionEntity Session { get; set; } = null!;

        public Guid PaymentTransactionId { get; set; }
        public PaymentTransactionEntity PaymentTransaction { get; set; } = null!;

        public decimal Price { get; set; }
        public TicketStatusEnum TicketStatus { get; set; }
    }
}
