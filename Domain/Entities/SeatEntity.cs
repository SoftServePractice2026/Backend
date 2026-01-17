using Domain.Entities.Enums;
using Domain.Primitives;

namespace Domain.Entities
{
    public class SeatEntity : Entity
    {
        public Guid HallId { get; set; }
        public HallEntity? Hall { get; set; }

        public int RowNumber { get; set; }
        public int SeatNumber { get; set; }
        public SeatTypeEnum SeatType { get; set; }
        public SeatStatusEnum SeatStatus { get; set; }
    }
}
