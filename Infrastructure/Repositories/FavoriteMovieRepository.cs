using Domain.Entities;
using Domain.Interfaces;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

public class FavoriteMovieRepository : IFavoriteMovieRepository
{
    private readonly CinemaDbContext _context;

    public FavoriteMovieRepository(CinemaDbContext context)
    {
        _context = context;
    }

    public Task<bool> ExistsAsync(Guid userId, Guid movieId, CancellationToken cancellationToken) =>
        _context.FavoriteMovies.AnyAsync(x => x.UserId == userId && x.MovieId == movieId, cancellationToken);

    public Task<List<Guid>> GetMovieIdsByUserAsync(Guid userId, CancellationToken cancellationToken) =>
        _context.FavoriteMovies
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.AddedAt)
            .Select(x => x.MovieId)
            .ToListAsync(cancellationToken);

    public Task<FavoriteMovieEntity?> GetByUserAndMovieAsync(Guid userId, Guid movieId, CancellationToken cancellationToken) =>
        _context.FavoriteMovies.FirstOrDefaultAsync(x => x.UserId == userId && x.MovieId == movieId, cancellationToken);

    public void Add(FavoriteMovieEntity entity) => _context.FavoriteMovies.Add(entity);

    public void Remove(FavoriteMovieEntity entity) => _context.FavoriteMovies.Remove(entity);
}