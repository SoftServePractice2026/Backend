using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IActorRepository
    {
        Task CreateActorAsync(ActorEntity actorEntity);
        Task DeleteActorAsync(ActorEntity actorEntity);
        Task UpdateActorAsync(ActorEntity actorEntity);
        Task<ActorEntity?> GetActorByIdAsync(Guid actorId);
        Task<IReadOnlyList<ActorEntity>> FindActorsByNameAsync(string actorName);
        Task<IReadOnlyList<ActorEntity>> FindActorsBySurnameAsync(string actorSurname);
        Task<IReadOnlyList<ActorEntity>> FindActorsByFullNameAsync(string fullName);
        Task<IReadOnlyList<ActorEntity>> GetActorEntitiesAsync();
        Task SaveChangesAsync();

    }
}
