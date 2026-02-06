namespace Application.Dtos.Movie;

public record MovieListItemDto(
    Guid Id, 
    int Duration,
    int AgeRating,
    decimal Rating,
    string Poster,
    string Title,
    List<Guid> GenreIds,
    List<Guid> ActorsIds,
    DateTime RentalStart,
    DateTime RentalEnd,
    string Description
    );