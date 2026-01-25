using Domain.Entities.Enums;

namespace Domain.Filters
{
    public class HallFilter
    {
        public bool? IsActive { get; init; }
        public HallSizeEnum? HallSize { get; init; }

        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}
