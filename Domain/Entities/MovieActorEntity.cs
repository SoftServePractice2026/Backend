using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class MovieCatalogActor
    {
        public Guid Id { get; set; }

        public Guid ActorId { get; set; }

        public Guid MovieId {  get; set; }

        public ActorEntity? Actor { get; set; }

        public MovieEntity? Movie { get; set; }
    }
}
