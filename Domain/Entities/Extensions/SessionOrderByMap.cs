using System.Linq.Expressions;

namespace Domain.Entities.Extensions;

public class SessionOrderByMap
{
    public static readonly IReadOnlyDictionary<string, Expression<Func<SessionEntity, object>>> Map
        = new Dictionary<string, Expression<Func<SessionEntity, object>>>
        {
            ["id"] = x => x.Id,
            ["starttime"] = x => x.StartTime,
            ["endtime"] = x => x.EndTime,
            ["status"] = x => x.SessionStatus,
            ["movieid"] = x => x.MovieId,
            ["hallid"] = x => x.HallId,
            
            ["movietitle"] = s => s.Movie.Title,
            ["hallname"] = s => s.Hall.Name
        };
}