using System;
using System.Collections.Generic;
using System.Text;

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

    public record ImportStatsDto(
        int Added,
        int Updated,
        int Failed,
        int Total
        );
}
