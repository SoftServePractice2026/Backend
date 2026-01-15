using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class SeatEntity
    {
        public Guid Id { get; set; }

        public int HallId { get; set; }

        public HallEntity? Hall { get; set; }

        public TicketEntity? Ticket { get; set; }

        public int RawNumber { get; set; }

        public int SeatNumber { get; set; }

        public SeatTypeEnum SeatType { get; set; }

        public bool IsActive { get; set; }


    }
}
