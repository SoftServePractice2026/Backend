using Domain.Primitives;

namespace Domain.Entities
{
    public class MovieActorEntity:Entity
    {
        public Guid ActorId { get; set; }
        public ActorEntity Actor { get; set; } = null!;

        public Guid MovieId {  get; set; }
        public MovieEntity Movie { get; set; } = null!;

        public string? CharacterName { get; set; }
    }
}
