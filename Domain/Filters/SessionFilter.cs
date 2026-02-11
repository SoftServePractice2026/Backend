using Domain.Entities.Enums;
using Shared.Common;

namespace Domain.Filters;

public class SessionFilter
{
    public Guid? MovieId { get; init; }
    public Guid? HallId { get; init; }
    public SessionStatusEnum? Status { get; init; }
    
    public DateTime? Date { get; init; }
    public DateTime? From { get; init; }
    public DateTime? To { get; init; }
    public string? MovieTitle { get; init; }

    public int PageNumber { get; init; }
    public int PageSize { get; init; }

    
    public string? OrderBy { get; init; }
    public SortDirection SortDirection { get; init; }
}
