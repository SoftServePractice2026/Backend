using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Domain.Entities.Extensions
{
    public static class ActorOrederByMap
    {
        public static readonly IReadOnlyDictionary<string, Expression<Func<ActorEntity, object>>> Map
       = new Dictionary<string, Expression<Func<ActorEntity, object>>>
       {
           ["id"] = x => x.Id,
           ["firstname"] = x => x.FirstName,
           ["lastname"] = x => x.LastName
       };
    }
}
