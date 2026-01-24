using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IHallRepository
    {
        void CreateHall(HallEntity hallEntity);
        void DeleteHall(HallEntity hallEntity);
        void UpdateHall(HallEntity hallEntity);

        Task<HallEntity?> GetHallByIdAsync(Guid hallId, CancellationToken cancellationToken);
        Task<HallEntity?> GetHallByNameAsync(string hallName, CancellationToken cancellationToken);
        Task<List<HallEntity>> GetHallsAsync(CancellationToken cancellationToken);
    }
}
