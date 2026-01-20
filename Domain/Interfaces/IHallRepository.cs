using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IHallRepository
    {
        Task CreateHallAsync(HallEntity hallEntity);
        Task DeleteHallAsync(HallEntity hallEntity);
        Task UpdateHallAsync(HallEntity hallEntity);
        Task<HallEntity?> GetHallByIdAsync(Guid hallId);
        Task<HallEntity?> GetHallByNameAsync(string hallName);
        Task<List<HallEntity>> GetHallEntitiesAsync();
        Task SaveChangesAsync();
    }
}
