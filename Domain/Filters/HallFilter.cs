using Domain.Entities.Enums;
using Shared.Common;

namespace Domain.Filters
{
    public class HallFilter
    {
        public bool? IsActive { get; init; }
        public HallSizeEnum? HallSize { get; init; }

        public DateTime? Date { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }

        public string? OrderBy { get; init; }
        public SortDirection SortDirection { get; init; }
    }
}
