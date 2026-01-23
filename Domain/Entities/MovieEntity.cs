using Domain.Primitives;
using Shared;

namespace Domain.Entities
{
    public class MovieEntity : AuditableEntity
    {
        private MovieEntity(
            string title, 
            string description, 
            string? poster, 
            int ageRating, 
            string language, 
            int duration,
            decimal? rating,
            DateTime rentalStartDate,
            DateTime rentalEndDate)
        {
            Title = title;
            Description = description;
            Poster = poster;
            AgeRating = ageRating;
            Language = language;
            Duration = duration;
            Rating = rating;
            RentalStartDate = rentalStartDate;
            RentalEndDate = rentalEndDate;
        }
        
        private MovieEntity() { }
        
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


        public static MovieEntity Create(
            string? poster, 
            string title, 
            string description, 
            int ageRating, 
            string language,
            int duration, 
            DateTime start,
            DateTime end)
        {

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be empty", nameof(title));
            }

            
            if (duration <= 0) throw new ArgumentException("Duration must be positive", nameof(duration));

            if (end <= start)
            {
                throw new ArgumentException("End date must be after start date", nameof(end));
            }

            var movie = new MovieEntity(
                title,
                description,
                poster,
                ageRating,
                language,
                duration,
                null,
                start,
                end);
            
            return movie;
        }
    }
    
}
