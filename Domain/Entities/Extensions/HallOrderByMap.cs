using System.Linq.Expressions;

namespace Domain.Entities.Extensions
{
    public static class HallOrderByMap
    {
        public static readonly IReadOnlyDictionary<string, Expression<Func<HallEntity, object>>> Map
        = new Dictionary<string, Expression<Func<HallEntity, object>>>
        {
            ["id"] = x => x.Id,
            ["name"] = x => x.Name,
            ["isactive"] = x => x.IsActive,
            ["hallsize"] = x => x.HallSize,
        };
    }
}
