using Domain.Entities.Interfaces;
using Domain.Primitives;

namespace Domain.Entities
{
    public class FavoriteMovieEntity : Entity
    {
        public Guid UserId { get; set; }
        public IUser ApplicationUser { get; set; } = null!;

        public Guid MovieId { get; set; }
        public MovieEntity Movie { get; set; } = null!;

        public DateTime AddedAt { get; set; }
    }
}
