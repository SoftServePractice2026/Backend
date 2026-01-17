using Domain.Entities.Enums;
using Domain.Entities.Interfaces;
using Domain.Primitives;

namespace Domain.Entities
{
    public class PaymentTransactionEntity : AuditableEntity
    {
        public Guid UserId { get; set; }
        public IUser ApplicationUser { get; set; } = null!;

        public string Currency { get; set; } = null!;
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

        public ICollection<TicketEntity> Tickets { get; set; } = new List<TicketEntity>();
    }
}
