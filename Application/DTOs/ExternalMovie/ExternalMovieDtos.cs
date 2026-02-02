using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Application.DTOs.ExternalMovieDto
{
    // Це "чиста" DTO, яку ти використовуєш у сервісі (залиш як є, або трохи спрости)
    public record ExternalMovieDto(
        int TmdbId,
        string Title,
        string Description,
        string Poster,
        int Duration,
        string Language,
        decimal Rating,
        bool AgeRating,
        List<string> Genres,       // Тут ми хочемо вже готові назви
        List<ExternalActorDto> Cast, // Тут вже готові актори
        DateTime ReleaseDate
    );

    public record ExternalActorDto(
        int TmdbId,
        string FullName,
        string PhotoUrl
    );

    // --- А ЦЕ НОВІ КЛАСИ ДЛЯ ЗЧИТУВАННЯ ВІДПОВІДІ TMDB ---

    public class TmdbMovieResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("overview")]
        public string Overview { get; set; } // Це Description

        [JsonPropertyName("poster_path")]
        public string PosterPath { get; set; }

        [JsonPropertyName("runtime")]
        public int? Runtime { get; set; } // TMDB іноді повертає null

        [JsonPropertyName("original_language")]
        public string Language { get; set; }

        [JsonPropertyName("vote_average")]
        public decimal VoteAverage { get; set; }

        [JsonPropertyName("adult")]
        public bool IsAdult { get; set; }

        [JsonPropertyName("release_date")]
        public string ReleaseDateString { get; set; } // Краще читати як строку, бо може бути пусто

        [JsonPropertyName("genres")]
        public List<TmdbGenre> Genres { get; set; }

        [JsonPropertyName("credits")]
        public TmdbCredits Credits { get; set; }
    }

    public class TmdbGenre
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class TmdbCredits
    {
        [JsonPropertyName("cast")]
        public List<TmdbCastItem> Cast { get; set; }
    }

    public class TmdbCastItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } // TMDB використовує "name", а не "FullName"

        [JsonPropertyName("profile_path")]
        public string ProfilePath { get; set; } // TMDB використовує "profile_path"
    }

    public record ImportStatsDto(int Added, int Updated, int Failed, int Total);
}
