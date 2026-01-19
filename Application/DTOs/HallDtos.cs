using Domain.Entities.Enums;

namespace Application.DTOs
{
    public record HallDetailsDto(
        Guid Id,
        string Name,
        bool IsActive,
        HallSizeEnum HallSize);

    public record CreateHallDto(
        string Name,
        bool IsActive,
        HallSizeEnum HallSize);
}
