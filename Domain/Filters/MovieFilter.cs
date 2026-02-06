using Shared.Common;

namespace Domain.Filters;

public class MovieFilter
{
    public string? Title { get; init; }
    public List<Guid>? GenreIds { get; init; }
    public List<Guid>? ActorsIds { get; init; }
    public int? MinAgeRating { get; init; }
    
    
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    
    public string? OrderBy { get; init; }

    public SortDirection SortDirection { get; init; } = SortDirection.Ascending;
}