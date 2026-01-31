using System.Linq.Expressions;

namespace Domain.Entities.Extensions;

public static class MovieOrderByMap
{
    public static readonly IReadOnlyDictionary<string, Expression<Func<MovieEntity, object>>> Map
        = new Dictionary<string, Expression<Func<MovieEntity, object>>>
        {
            ["id"] = x => x.Id,
            ["title"] = x => x.Title,
            ["rating"] = x => x.Rating!,
            ["agerating"] = x => x.AgeRating,
            ["duration"] = x => x.Duration,
            ["startdate"] = x => x.RentalStartDate,
            ["enddate"] = x => x.RentalEndDate
        };
}