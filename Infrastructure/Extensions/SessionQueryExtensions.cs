using Domain.Entities;
using Domain.Filters;

namespace Infrastructure.Extensions;


public static class SessionQueryExtensions
{
    public static IQueryable<SessionEntity> ApplyFilters(
        this IQueryable<SessionEntity> query,
        SessionFilter filter)
    {
        if (filter.MovieId.HasValue)
            query = query.Where(s => s.MovieId == filter.MovieId.Value);

        if (filter.HallId.HasValue)
            query = query.Where(s => s.HallId == filter.HallId.Value);

        if (filter.Status.HasValue)
            query = query.Where(s => s.SessionStatus == filter.Status.Value);

        if (filter.From.HasValue)
            query = query.Where(s => s.StartTime >= filter.From.Value);
        
        if (filter.To.HasValue)
            query = query.Where(s => s.StartTime <= filter.To.Value);

        if (filter.Date.HasValue)
        {
            var targetDate = DateTime.SpecifyKind(filter.Date.Value.Date, DateTimeKind.Utc);
            query = query.Where(s => s.StartTime.Date == targetDate);
        }

        if (!string.IsNullOrWhiteSpace(filter.MovieTitle))
        {
            var title = filter.MovieTitle.Trim();
            query = query.Where(s => s.Movie.Title.Contains(title));
        }
        return query;
    }
}