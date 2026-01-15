using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ActorEntity
    {
        public Guid Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public ICollection<MovieCatalogActor> ActorsInMovies { get; set; }    
    }
}
