using Domain.Entities.Enums;
using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Domain.Entities
{
    public class SessionEntity:Entity
    {
        public Guid MovieId { get; set; }

        public Guid HallId {  get; set; }

        public MovieEntity? Movie { get; set; }

        public HallEntity? Hall { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set;}

        public StatusEnum Status { get; set; }

        public ICollection<TicketEntity> Tickets { get; set; }
    }
}
