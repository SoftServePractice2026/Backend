using Domain.Primitives;

namespace Domain.Entities
{
    public class ActorEntity : Entity
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public ICollection<MovieActorEntity> ActorsInMovies { get; set; } = new List<MovieActorEntity>();
    }
}
