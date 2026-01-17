using Domain.Entities.Enums;
using Domain.Primitives;

namespace Domain.Entities
{
    public class PaymentTransaction : AuditableEntity
    {
        public Guid TicketId { get; set; }
        public TicketEntity? Ticket { get; set; }

        public string UserId { get; set; } = null!; // Because IdentityUser.Id -> string
        public string Currency { get; set; } = null!;
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}
