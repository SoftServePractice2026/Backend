namespace Domain.Filters;

public class MovieFilter
{
    public string? Title { get; init; }
    
    public Guid? GenreId { get; init; }
    
    public int? MinAgeRating { get; init; }
    
    
    
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}