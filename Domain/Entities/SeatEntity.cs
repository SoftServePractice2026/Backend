using Domain.Entities.Enums;
using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class SeatEntity:Entity
    {
        public int HallId { get; set; }

        public HallEntity? Hall { get; set; }

        public TicketEntity? Ticket { get; set; }

        public int RawNumber { get; set; }

        public int SeatNumber { get; set; }

        public SeatTypeEnum SeatType { get; set; }

        public SeatStatusEnum Status { get; set; }


    }
}
