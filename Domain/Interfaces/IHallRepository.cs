using Domain.Entities;
using Domain.Filters;

namespace Domain.Interfaces
{
    public interface IHallRepository
    {
        void CreateHall(HallEntity hallEntity);
        void DeleteHall(HallEntity hallEntity);
        void UpdateHall(HallEntity hallEntity);

        Task<bool> ExistsAsync(Guid hallId, CancellationToken cancellationToken);
        Task<HallEntity?> GetHallByIdAsync(Guid hallId, CancellationToken cancellationToken);
        Task<HallEntity?> GetHallByNameAsync(string hallName, CancellationToken cancellationToken);
        Task<List<HallEntity>> GetFilteredHallsAsync(HallFilter hallFilter, CancellationToken cancellationToken);
        Task<int> CountFilteredAsync(HallFilter hallFilter, CancellationToken cancellationToken);
    }
}
