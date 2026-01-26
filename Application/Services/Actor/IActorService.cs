using Application.DTOs;
using Application.DTOs.Actor;
using Application.Services.Hall;
using AutoMapper;
using Domain.Entities;
using Domain.Filters;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared;
using System.Reflection.Metadata.Ecma335;
namespace Application.Services.Actor
{
    public interface IActorService
    {
        Task<Result<ActorDetailsDto>> CreateActorAsync(ActorCreateDto dto, CancellationToken cancellationToken);
        Task<Result<ActorDetailsDto>> UpdateActorAsync(Guid targetId, ActorUpdateDto dto, CancellationToken cancellationToken);
        Task<Result<bool>> DeleteActorAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<ActorDetailsDto>> GetActorByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<(List<ActorListItemDto> Actors, int TotalCount)>> GetFilteredActorsAsync(ActorFilterDto actorFilterDto, CancellationToken cancellationToken);
    }
}
