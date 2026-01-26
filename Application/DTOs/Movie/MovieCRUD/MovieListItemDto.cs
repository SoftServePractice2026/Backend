namespace Application.Dtos.Movie;

public record MovieListItemDto(
    Guid Id, 
    string Title,
    Guid GenreId);