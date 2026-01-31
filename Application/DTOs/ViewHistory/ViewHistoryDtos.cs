using Domain.Entities.Enums;
using Application.DTOs.Common;
using Application.Interfaces;
using Shared.Common;

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
        String? OrderBy,
        SortDirection SortDirection = SortDirection.Ascending) : PaginationBaseDto, ISortable;
}

