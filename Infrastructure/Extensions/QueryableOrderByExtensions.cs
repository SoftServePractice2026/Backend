using Shared.Common;
using System.Linq.Expressions;

namespace Infrastructure.Extensions
{
    public static class QueryableOrderByExtensions
    {
        public static IQueryable<T> ApplyOrderBy<T>(
            this IQueryable<T> query,
            string? orderBy,
            SortDirection direction,
            IReadOnlyDictionary<string, Expression<Func<T, object>>> columns)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return query;

            if (!columns.TryGetValue(orderBy.ToLower(), out var expression))
                return query;

            return direction == SortDirection.Descending
                ? query.OrderByDescending(expression)
                : query.OrderBy(expression);
        }
    }
}
