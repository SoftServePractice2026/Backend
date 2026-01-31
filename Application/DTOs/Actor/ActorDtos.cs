using Application.DTOs.Common;
using Application.Interfaces;
using Shared.Common;

namespace Application.DTOs.Actor
{
    public record ActorDetailsDto(
           Guid Id,
           string FirstName,
           string LastName
           );


    public record ActorCreateDto(
        string FirstName,
        string LastName
        );

    public record ActorUpdateDto(
        string FirstName,
        string LastName
        );

    public record ActorListItemDto(
        Guid Id,
        string FirstName,
        string LastName
        );

    public record ActorFilterDto(
       string? SearchTerm,
       Guid? MovieId,
       string? OrderBy,
       SortDirection SortDirection = SortDirection.Ascending) : PaginationBaseDto, ISortable;
}
