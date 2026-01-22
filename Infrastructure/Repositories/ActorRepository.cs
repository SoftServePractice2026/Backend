using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class ActorRepository : IActorRepository
    {
        private readonly CinemaDbContext _context;
        
        public ActorRepository(CinemaDbContext context)
        {
            _context = context;
        }
        public async Task CreateActorAsync(ActorEntity actorEntity)
        {
            await _context.Actors.AddAsync(actorEntity);
        }

        public Task DeleteActorAsync(ActorEntity actorEntity)
        {
             _context.Actors.Remove(actorEntity);
            return Task.CompletedTask;
        }

        public Task UpdateActorAsync(ActorEntity actorEntity)
        {
            _context.Actors.Update(actorEntity);
            return Task.CompletedTask;
        }

        public async Task<IReadOnlyList<ActorEntity>> FindActorsByFullNameAsync(string fullName)
        {
            return await _context.Actors
           .AsNoTracking()
           .Where(a => EF.Functions.ILike(a.FirstName + " " + a.LastName, fullName + "%"))
           .ToListAsync();
        }

        public async Task<IReadOnlyList<ActorEntity>> FindActorsByNameAsync(string actorName)
        {
            return await _context.Actors
           .Where(a => EF.Functions.ILike(a.FirstName, actorName + "%"))
           .ToListAsync();
        }
      

        public async Task<IReadOnlyList<ActorEntity>> FindActorsBySurnameAsync(string actorSurname)
        {
            return await _context.Actors
           .Where(a => EF.Functions.ILike(a.LastName, actorSurname + "%"))
           .ToListAsync();
        }

        public Task<ActorEntity?> GetActorByIdAsync(Guid actorId) => _context.Actors.FirstOrDefaultAsync(x => x.Id == actorId);
        

        public async Task<IReadOnlyList<ActorEntity>> GetActorEntitiesAsync() => await _context.Actors.ToListAsync();
        

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
       
    }
}
