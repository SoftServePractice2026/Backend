using Domain.Entities.Enums;

namespace Domain.Filters
{
    public class ViewHistoryFilter
    {
        public Guid? UserId { get; init; }
        public Guid? SessionId { get; init; }
        
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}