using Application.DTOs.Common;
using Domain.Entities.Enums;

namespace Application.DTOs
{
    public record SessionCreateDto(
        Guid MovieId,
        Guid HallId,
        DateTime StartTime,
        DateTime EndTime,
        SessionStatusEnum SessionStatus
    );
    
    
    public record SessionUpdateDto(
        DateTime? StartTime,
        DateTime? EndTime,
        SessionStatusEnum? SessionStatus
    );
    
    
    public record SessionListItemDto(
        Guid Id,
        string MovieTitle,
        Guid MovieId,
        string HallName,
        Guid HallId,
        DateTime StartTime,
        DateTime EndTime,
        SessionStatusEnum SessionStatus
    );
    
    
    public record SessionFilterDto(
        Guid? MovieId,
        Guid? HallId,
        SessionStatusEnum? Status,
        DateTime? From,
        DateTime? To,
        string? MovieTitle
    ) : PaginationBaseDto;
    
    
    public record SessionFilterResultDto(
        List<SessionListItemDto> Sessions,
        int TotalCount
    );
}