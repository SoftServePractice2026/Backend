using Domain.Primitives;

namespace Domain.Entities
{
    public class MovieActorEntity:Entity
    {
        public Guid ActorId { get; set; }
        public ActorEntity? Actor { get; set; }

        public Guid MovieId {  get; set; }
        public MovieEntity? Movie { get; set; }

        public string? CharacterName { get; set; }
    }
}
