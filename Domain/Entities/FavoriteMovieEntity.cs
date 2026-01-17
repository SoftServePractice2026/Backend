using Domain.Primitives;

namespace Domain.Entities
{
    public class FavoriteMovieEntity : Entity
    {
        public string UserId { get; set; } = null!; // Because IdentityUser.Id -> string
        public DateTime AddedAt { get; set; }

        public Guid MovieId { get; set; }
        public MovieEntity? Movie { get; set; }
    }
}
