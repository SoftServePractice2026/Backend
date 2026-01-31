using Application.DTOs.Common;
using Application.Interfaces;
using Domain.Entities.Enums;
using Shared.Common;

namespace Application.DTOs
{
    public record HallDetailsDto(
        Guid Id,
        string Name,
        bool IsActive,
        HallSizeEnum HallSize);
    
    public record HallCreateDto(
        string Name,
        HallSizeEnum HallSize);

    public record HallUpdateDto(
        string Name,
        bool IsActive,
        HallSizeEnum HallSize);

    
    public record HallListItemDto(
        Guid Id,
        string Name,
        bool IsActive,
        HallSizeEnum HallSize);

    
    public record HallFilterDto(
        bool? IsActive,
        HallSizeEnum? HallSize,
        string? OrderBy,
        SortDirection SortDirection = SortDirection.Ascending) : PaginationBaseDto, ISortable; 
}
