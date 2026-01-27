using Domain.Entities.Enums;

namespace Domain.Filters;
public record SessionFilter(
    Guid? MovieId,
    Guid? HallId,
    SessionStatusEnum? Status,
    DateTime? From,
    DateTime? To,
    string? MovieTitle,
    int PageNumber,
    int PageSize
);