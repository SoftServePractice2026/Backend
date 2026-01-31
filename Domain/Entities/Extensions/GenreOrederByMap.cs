using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Domain.Entities.Extensions
{
    public static class GenreOrederByMap
    {
        public static readonly IReadOnlyDictionary<string, Expression<Func<GenreEntity, object>>> Map
        = new Dictionary<string, Expression<Func<GenreEntity, object>>>
        {
            ["id"] = x => x.Id,
            ["name"] = x => x.Name,
        };
    }
}
