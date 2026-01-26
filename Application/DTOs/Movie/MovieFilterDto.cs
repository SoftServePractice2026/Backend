namespace Application.Dtos.Movie;

public record MovieFilterDto(
    string? Title, 
    Guid? GenreId, 
    int? MinAgeRating,
    int Page = 1, 
    int PageSize = 10);