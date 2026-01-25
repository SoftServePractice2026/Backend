using Application.DTOs;
using Shared;

namespace Application.Services.Hall
{
    public interface IHallService
    {
        Task<Result<HallDetailsDto>> CreateHallAsync(HallCreateDto dto, CancellationToken cancellationToken);
        Task<Result<HallDetailsDto>> UpdateHallAsync(Guid targetId, HallUpdateDto dto, CancellationToken cancellationToken);
        Task<Result<bool>> DeleteHallAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<HallDetailsDto>> GetHallByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<HallDetailsDto>> GetHallByNameAsync(string name, CancellationToken cancellationToken);
        Task<Result<(List<HallListItemDto> Halls, int TotalCount)>> GetFilteredHallsAsync(HallFilterDto hallFilterDto, CancellationToken cancellationToken);
    }
}
