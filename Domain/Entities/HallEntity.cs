using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class HallEntity:Entity
    {
        public string? Name { get; set; }

        public int Raws { get; set; }

        public int SeatsPerRaw { get; set; }

        public bool IsActive { get; set; }

        public ICollection<SessionEntity> Sessions { get; set; }

        public ICollection<SeatEntity> Seats { get; set; }
    }
}
