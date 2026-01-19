using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IHallRepository
    {
        Task<Guid?> CreateHallAsync(HallEntity hallEntity);
    }
}
