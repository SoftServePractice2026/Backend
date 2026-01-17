using Domain.Primitives;

namespace Domain.Entities
{
    public class ViewHistoryEntity:Entity
    {
        public string UserId { get; set; } = null!; //Because IdentityUser.Id -> string
        public DateTime ViewedAt { get; set; }

        public Guid SessionId { get; set; }
        public SessionEntity? Movie { get; set; }
    }
}
