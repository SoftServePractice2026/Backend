using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace Domain.Entities
{
    public class TicketEntity:Entity
    {
        public Guid UserId { get; set; }

        public Guid SeatId { get; set; }

        public Guid SessionId { get; set; }

        //public UserEntity User { get; set; }

        public SessionEntity? Session { get; set; }

        public SeatEntity? Seat { get; set; }

        public DateTime? CreatedAt { get; set; }

        public decimal Price { get; set; }

    }
}
