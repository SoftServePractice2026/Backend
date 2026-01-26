using Domain.Entities.Enums;

namespace Application.DTOs
{
    public record ViewHistoryDetailsDto(
        Guid Id,
        Guid UserId,
        Guid SessionId,
        DateTime ViewedAt);

    public record ViewHistoryCreateDto(
        Guid UserId,
        Guid SessionId,
        DateTime ViewedAt);

    public record ViewHistoryUpdateDto(
        Guid UserId,
        Guid SessionId,
        DateTime ViewedAt);

    public record ViewHistoryListItemDto(
        Guid Id,
        Guid SessionId,
        DateTime ViewedAt);
    
    public record ViewHistoryFilterDto(
        Guid? UserId,
        Guid? SessionId,
        int Page = 1,
        int PageSize = 10);
}

