using Domain.Entities.Enums;

namespace Application.DTOs
{
    public record HallDetailsDto(
        Guid Id,
        string Name,
        bool IsActive,
        HallSizeEnum HallSize);

    public record HallCreateDto(
        string Name,
        bool IsActive,
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
}
