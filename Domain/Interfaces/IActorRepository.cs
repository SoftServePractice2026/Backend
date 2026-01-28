using Domain.Entities;
using Domain.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IActorRepository
    {
        void CreateActor(ActorEntity actorEntity);
        void DeleteActor(ActorEntity actorEntity);
        void UpdateActor(ActorEntity actorEntity);
        Task<ActorEntity?> GetActorByIdAsync(Guid actorId, CancellationToken cancellationToken);
        Task<List<ActorEntity>> GetFilteredActorsAsync(ActorFilter actorFilter, CancellationToken cancellationToken);
        Task<int> CountFilteredAsync(ActorFilter actorFilter, CancellationToken cancellationToken);

    }
}
