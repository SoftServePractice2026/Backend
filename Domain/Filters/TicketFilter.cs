using Domain.Entities.Enums;
using Shared.Common;

namespace Domain.Filters
{
    public class TicketFilter
    {
        public Guid? UserId { get; init; }
        public TicketStatusEnum? TicketStatus { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        
        public string? OrderBy { get; init; }
        public SortDirection SortDirection { get; init; }
        
        
        
    } 
}

