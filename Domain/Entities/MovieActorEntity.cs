using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class MovieActorEntity:Entity
    {
        public Guid ActorId { get; set; }

        public Guid MovieId {  get; set; }

        public ActorEntity? Actor { get; set; }

        public MovieEntity? Movie { get; set; }
    }
}
