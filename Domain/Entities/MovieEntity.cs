using Domain.Entities.Enums;
using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Domain.Entities
{
    public class MovieEntity:Entity
    {
        public string? Poster { get; set; }

        public string? Description { get; set; }

        public CategoryEnum Category { get; set; }

        public GenreEnum Genre { get; set; }

        public int Duration { get; set; }

        public decimal Rating { get; set; }

        public DateTime? StartedAt { get; set; }

        public DateTime? EndedAt {  get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<MovieActorEntity> ActorsInMovies { get; set; }

        public ICollection<ViewHistoryEntity> MoviesViewed { get; set; }

        public ICollection<SessionEntity> Sessions { get; set; }
    }
}
