using Domain.Entities.Interfaces;
using Domain.Primitives;

namespace Domain.Entities
{
    public class ViewHistoryEntity : Entity
    {
        public Guid UserId { get; set; }
        public IUser ApplicationUser { get; set; } = null!;

        public DateTime ViewedAt { get; set; }

        public Guid SessionId { get; set; }
        public SessionEntity? Session { get; set; }
    }
}
