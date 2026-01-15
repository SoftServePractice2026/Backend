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
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;

        public CategoryEnum Category { get; set; } 

        public GenreEnum Genre { get; set; }

        public int Duration { get; set; }

        public decimal? Rating { get; set; }

        public DateTime RentalStartDate { get; set; }
        
        public DateTime RentalEndDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public ICollection<MovieActorEntity> ActorsInMovies { get; set; } = new List<MovieActorEntity>();
        public ICollection<ViewHistoryEntity> MoviesViewed { get; set; } = new List<ViewHistoryEntity>();
        public ICollection<SessionEntity> Sessions { get; set; } = new List<SessionEntity>();
    }
}
