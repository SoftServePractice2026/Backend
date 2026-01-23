namespace Application.Dtos.Movie;

public record MovieDetailsDto(
    Guid Id,
    string Title,
    string Description,
    string Poster,
    int AgeRating,
    decimal Rating,
    DateTime ReleaseDate,
    List<string> Genres);
