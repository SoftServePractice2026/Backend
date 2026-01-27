using Domain.Entities.Enums;
using Shared.Common;

namespace Domain.Filters
{
    public class ViewHistoryFilter
    {
        public Guid? UserId { get; init; }
        public Guid? SessionId { get; init; }
        
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        
        public string? OrderBy { get; init; }
        public SortDirection SortDirection { get; init; }
    }
}