using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Filters
{
    public class ActorFilter
    {
        public string? SearchTerm { get; init; }

        public Guid? MovieId { get; init; }

        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}
