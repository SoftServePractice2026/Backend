using System.Linq.Expressions;
using Domain.Entities;

namespace Domain.Entities.Extensions
{
    public static class ViewHistoryOrederByMap
    {
        public static readonly IReadOnlyDictionary<string, Expression<Func<ViewHistoryEntity, object>>> Map
            = new Dictionary<string, Expression<Func<ViewHistoryEntity, object>>>
            {
                ["id"] = x => x.Id,
                ["viewedat"] = x => x.ViewedAt,
                ["userid"] = x => x.UserId,
                ["movieid"] = x => x.SessionId,
            };
    }
}