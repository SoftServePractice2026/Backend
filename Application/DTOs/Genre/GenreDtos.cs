using Application.DTOs.Common;
using Application.Interfaces;
using Shared.Common;

namespace Application.DTOs.Genre
{
    public record GenreDetailsDto(
           Guid Id,
           string Name
           );


    public record GenreCreateDto(
        string Name
        );

    public record GenreUpdateDto(
        string Name
        );

    public record GenreListItemDto(
        Guid Id,
        string Name
        );

    public record GenreFilterDto(
        string? Name,
        string? OrderBy,
        SortDirection SortDirection = SortDirection.Ascending) : PaginationBaseDto, ISortable; 
}
