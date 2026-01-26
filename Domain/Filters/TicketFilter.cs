using Domain.Entities.Enums;

namespace Domain.Filters
{
    public class TicketFilter
    {
        public Guid? UserId { get; set; }
        public TicketStatusEnum? TicketStatus { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        
    } 
}

