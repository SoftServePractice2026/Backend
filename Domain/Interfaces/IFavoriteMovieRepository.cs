using Domain.Entities;
namespace Domain.Interfaces;

public interface IFavoriteMovieRepository
{
    Task<bool> ExistsAsync(Guid userId, Guid movieId, CancellationToken cancellationToken);
    Task<List<Guid>> GetMovieIdsByUserAsync(Guid userId, CancellationToken cancellationToken);
    void Add(FavoriteMovieEntity entity);
    void Remove(FavoriteMovieEntity entity);
    Task<FavoriteMovieEntity?> GetByUserAndMovieAsync(Guid userId, Guid movieId, CancellationToken cancellationToken);
}