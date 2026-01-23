namespace Application.Dtos.Movie;

public record CreateMovieDto(
    string Title, 
    string Description, 
    string? Poster,
    string Language, 
    int AgeRating,
    int Duration,
    DateTime RentalStartDate,
    DateTime RentalEndDate,
    List<Guid> GenreIds
    );