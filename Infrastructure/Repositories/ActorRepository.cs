using Domain.Entities;
using Domain.Entities.Extensions;
using Domain.Filters;
using Domain.Interfaces;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class ActorRepository : IActorRepository
    {
        private readonly CinemaDbContext _context;
        private readonly ILogger<ActorRepository> _logger;

        public ActorRepository(CinemaDbContext context, ILogger<ActorRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void CreateActor(ActorEntity actorEntity) => _context.Actors.Add(actorEntity);
        

        public void DeleteActor(ActorEntity actorEntity) => _context.Actors.Remove(actorEntity);
        

        public void UpdateActor(ActorEntity actorEntity) => _context.Actors.Update(actorEntity);

        public async Task<ActorEntity?> GetByTmdbIdAsync(int tmdbId)
        {
            return await _context.Actors
                .FirstOrDefaultAsync(a => a.TmdbId == tmdbId);
        }

        public async Task<ActorEntity?> GetActorByIdAsync(Guid actorId, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Actors.FindAsync([actorId], cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, $"Db error while getting actor by id: {actorId}");
                throw;
            }
        }

        public async Task<List<ActorEntity>> GetFilteredActorsAsync(ActorFilter actorFilter, CancellationToken cancellationToken)
        {
            var query = _context.Actors.AsQueryable();
            if (!string.IsNullOrWhiteSpace(actorFilter.SearchTerm))
            {
                var term = actorFilter.SearchTerm.Trim();
                query = query.Where(a => a.FirstName.Contains(term) || a.LastName.Contains(term));
            }

            if (actorFilter.MovieId.HasValue)
            {
                query = query.Where(a => a.ActorsInMovies.Any(m => m.MovieId == actorFilter.MovieId));
            }

     
            query = query.ApplyOrderBy(
                actorFilter.OrderBy,
                actorFilter.SortDirection,
                ActorOrderByMap.Map);

       
            var skip = (actorFilter.PageNumber - 1) * actorFilter.PageSize;
            query = query.Skip(skip).Take(actorFilter.PageSize);

            try
            {
                return await query.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Database error while filtering actors");
                throw;
            }
        }

        public async Task<int> CountFilteredAsync(ActorFilter actorFilter, CancellationToken cancellationToken)
        {
            var query = _context.Actors.AsQueryable();
            if (!string.IsNullOrWhiteSpace(actorFilter.SearchTerm))
            {
                var term = actorFilter.SearchTerm.Trim();
                query = query.Where(a => a.FirstName.Contains(term) || a.LastName.Contains(term));
            }

            if (actorFilter.MovieId.HasValue)
            {
                query = query.Where(a => a.ActorsInMovies.Any(m => m.MovieId == actorFilter.MovieId));
            }

            try
            {
                return await query.CountAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Database error while count filtered actors");
                throw;
            }
        }
    }
}
