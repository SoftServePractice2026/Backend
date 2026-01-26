using Domain.Entities.Enums;
using Domain.Primitives;

namespace Domain.Entities
{
    public class HallEntity : Entity
    {
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public HallSizeEnum HallSize { get; set; }

        public ICollection<SessionEntity> Sessions { get; set; } = new List<SessionEntity>();
        public ICollection<SeatEntity> Seats { get; set; } = new List<SeatEntity>();

    }
}
