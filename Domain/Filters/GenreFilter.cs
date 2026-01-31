using Shared.Common;

namespace Domain.Filters
{
    public class GenreFilter
    {
        public string? Name { get; init; }    
        public int PageNumber { get; init; }
        public int PageSize { get; init; }

        public string? OrderBy { get; init; }
        public SortDirection SortDirection { get; init; }
    }
}
