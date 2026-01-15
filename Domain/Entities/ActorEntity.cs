using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ActorEntity: Entity
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public ICollection<MovieActorEntity> ActorsInMovies { get; set; }    
    }
}
