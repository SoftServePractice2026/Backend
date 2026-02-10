namespace Application.Dtos.Movie;

public record UpdateMovieDto(
    Guid Id,
    string Title,
    string Description,
    string? Poster,
    int AgeRating,
    string Language,
    int Duration,
    int? Year,
    List<string>? Formats
    );