using Domain.Primitives;

namespace Domain.Entities
{
    public class ActorEntity : Entity
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public int TmdbId { get; set; }
        public string Photo {  get; set; }
        public ICollection<MovieActorEntity> ActorsInMovies { get; set; } = new List<MovieActorEntity>();
    }
}
