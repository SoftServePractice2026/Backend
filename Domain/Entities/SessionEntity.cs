using Domain.Entities.Enums;
using Domain.Primitives;

namespace Domain.Entities
{
    public class SessionEntity : AuditableEntity
    {
        public Guid MovieId { get; set; }
        public MovieEntity? Movie { get; set; }

        public Guid HallId { get; set; }
        public HallEntity? Hall { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public SessionStatusEnum SessionStatus { get; set; }

        public ICollection<TicketEntity> Tickets { get; set; } = new List<TicketEntity>();
    }
}
