using Domain.Entities.Enums;
using Domain.Primitives;

namespace Domain.Entities
{
    public class TicketEntity : Entity
    {
        public string UserId { get; set; } = null!; //Because IdentityUser.Id -> string

        public Guid SeatId { get; set; }
        public SeatEntity? Seat { get; set; }

        public Guid SessionId { get; set; }
        public SessionEntity? Session { get; set; }

        public decimal Price { get; set; }
        public TicketStatusEnum TicketStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
