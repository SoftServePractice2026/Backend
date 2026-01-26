using Domain.Interfaces;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CinemaDbContext _context;

        public UnitOfWork(CinemaDbContext context)
        {
            _context = context;
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
        {
            return _context.SaveChangesAsync(ct);
        }
    }
}
