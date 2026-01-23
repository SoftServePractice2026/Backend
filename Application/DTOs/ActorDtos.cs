using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
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
}
