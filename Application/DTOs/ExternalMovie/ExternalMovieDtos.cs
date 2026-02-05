using System.Text.Json.Serialization;

namespace Application.DTOs.ExternalMovieDto
{
    public record ExternalMovieDto(
        int TmdbId,
        string Title,
        string Description,
        string Poster,
        int Duration,
        string Language,
        decimal Rating,
        bool AgeRating,
        List<string> Genres,
        List<ExternalActorDto> Cast,
        DateTime ReleaseDate
    );

    public record ExternalActorDto(
        int TmdbId,
        string FullName,
        string PhotoUrl
    );


    public class TmdbMovieResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = null!;

        [JsonPropertyName("overview")]
        public string Overview { get; set; } = null!;

        [JsonPropertyName("poster_path")]
        public string PosterPath { get; set; } = null!;

        [JsonPropertyName("runtime")]
        public int? Runtime { get; set; }

        [JsonPropertyName("original_language")]
        public string Language { get; set; } = null!;

        [JsonPropertyName("vote_average")]
        public decimal VoteAverage { get; set; }

        [JsonPropertyName("adult")]
        public bool IsAdult { get; set; }

        [JsonPropertyName("release_date")]
        public string ReleaseDateString { get; set; } = null!;

        [JsonPropertyName("genres")]
        public List<TmdbGenre> Genres { get; set; } = new List<TmdbGenre>();

        [JsonPropertyName("credits")]
        public TmdbCredits Credits { get; set; } = default!;
    }

    public class TmdbGenre
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;
    }

    public class TmdbCredits
    {
        [JsonPropertyName("cast")]
        public List<TmdbCastItem> Cast { get; set; } = new List<TmdbCastItem>();
    }

    public class TmdbCastItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("profile_path")]
        public string? ProfilePath { get; set; }
    }

    public record ImportStatsDto(int Added, int Updated, int Failed, int Total);
}
