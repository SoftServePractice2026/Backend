using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
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
}
