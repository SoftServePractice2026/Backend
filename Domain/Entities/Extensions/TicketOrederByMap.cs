using System.Linq.Expressions;
using Domain.Entities;

namespace Domain.Entities.Extensions
{
    public static class TicketOrederByMap
    {
        public static readonly IReadOnlyDictionary<string, Expression<Func<TicketEntity, object>>> Map
            = new Dictionary<string, Expression<Func<TicketEntity, object>>>
            {
                ["id"] = x => x.Id,
                ["price"] = x => x.Price,
                ["status"] = x => x.TicketStatus,
                ["seatid"] = x => x.SeatId,
                ["sessionid"] = x => x.SessionId,
                ["createdat"] = x => x.CreatedAt,
            };
    }
}