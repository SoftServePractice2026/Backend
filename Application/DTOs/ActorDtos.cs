using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public record ActorDetailsDto(
        Guid Id,
        string Name,
        string Surname
        );

    public record ActorCreateDto(
        string Name,
        string Surname
        );

    public record ActorUpdateDto(
        string Name,
        string Surname
        );

    public record ActorListItemDto(
        Guid Id,
        string Name,
        string Surname
        );
}
