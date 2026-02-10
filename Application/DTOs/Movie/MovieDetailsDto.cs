namespace Application.Dtos.Movie;

public record MovieDetailsDto(
    Guid Id,
    string Title,
    string Description,
    string Poster,
    int AgeRating,
    decimal Rating,
    int Duration,
    int? Year,
    List<string> Formats,
    DateTime RentalStartDate,
    List<string> Actors,
    List<string> Genres
    );
