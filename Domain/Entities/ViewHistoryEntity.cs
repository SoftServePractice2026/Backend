using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ViewHistoryEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid MovieId {  get; set; }

       // public UserEntity User { get; set; }

        public MovieEntity? Movie { get; set; }  

        public DateTime ViewedAt { get; set; }
    }
}
