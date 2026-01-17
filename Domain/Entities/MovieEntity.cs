using Domain.Primitives;

namespace Domain.Entities
{
    public class MovieEntity : AuditableEntity
    {
        public string? Poster { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int AgeRating { get; set; }
        public string Language { get; set; } = null!;
        public int Duration { get; set; }
        public decimal? Rating { get; set; }
        public DateTime RentalStartDate { get; set; }
        public DateTime RentalEndDate { get; set; }

        public ICollection<GenreEntity> Genres { get; set; } = new List<GenreEntity>();
        public ICollection<MovieActorEntity> ActorsInMovies { get; set; } = new List<MovieActorEntity>();
        public ICollection<SessionEntity> Sessions { get; set; } = new List<SessionEntity>();
        public ICollection<FavoriteMovieEntity> FavoriteMovies { get; set; } = new List<FavoriteMovieEntity>();
    }
}
