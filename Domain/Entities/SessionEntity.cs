using Domain.Entities.Enums;
using Domain.Primitives;

namespace Domain.Entities
{
    public class SessionEntity : AuditableEntity
    {
        public Guid MovieId { get; set; }
        public MovieEntity Movie { get; set; } = null!;

        public Guid HallId { get; set; }
        public HallEntity Hall { get; set; } = null!;

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public SessionStatusEnum SessionStatus { get; set; }

        public ICollection<TicketEntity> Tickets { get; set; } = new List<TicketEntity>();
        public ICollection<ViewHistoryEntity> ViewHistories { get; set; } = new List<ViewHistoryEntity>();
    }
}
