using Application.DTOs;
using Application.DTOs.Actor;
using AutoMapper;
using Domain.Entities;
using Domain.Filters;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Application.Services.Actor
{
    public class ActorService : IActorService
    {
        private readonly IActorRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ActorService> _logger;

        public ActorService(IActorRepository repository, IMapper mapper, IUnitOfWork unitOfWork, ILogger<ActorService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<ActorDetailsDto>> CreateActorAsync(ActorCreateDto dto, CancellationToken cancellationToken)
        {
            var actor = _mapper.Map<ActorEntity>(dto);

            _repository.CreateActor(actor);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = _mapper.Map<ActorDetailsDto>(actor);

            return Result<ActorDetailsDto>.Success(resultDto);
        }

        public async Task<Result<bool>> DeleteActorAsync(Guid id, CancellationToken cancellationToken)
        {
            var actor = await _repository.GetActorByIdAsync(id, cancellationToken);

            if (actor is null)
            {
                var actorErorr = Error.NotFound("actor.not.found", $"Actor with id: {id} not found");
                _logger.LogWarning("Delete actor not found. ActorId={ActorId}, Code = {Code}", id, actorErorr.Code);
                return Result<bool>.Fail(actorErorr);
            }

            _repository.DeleteActor(actor);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<ActorDetailsDto>> GetActorByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var actor = await _repository.GetActorByIdAsync(id, cancellationToken);

            if (actor is null)
            {
                var actorErorr = Error.NotFound("actor.not.found", $"Actor with id: {id} not found");
                _logger.LogWarning("Get actor by id not found. ActorId={ActorId}, Code = {Code}", id, actorErorr.Code);
                return Result<ActorDetailsDto>.Fail(actorErorr);
            }

            var actorDto = _mapper.Map<ActorDetailsDto>(actor);

            return Result<ActorDetailsDto>.Success(actorDto);
        }

        public async Task<Result<(List<ActorListItemDto> Actors, int TotalCount)>> GetFilteredHallsAsync(ActorFilterDto actorFilterDto, CancellationToken cancellationToken)
        {
            var actorFilter = _mapper.Map<ActorFilter>(actorFilterDto);

            var actors = await _repository.GetFilteredActorsAsync(actorFilter, cancellationToken);

            if (!actors.Any())
            {
                var actorsErorr = Error.NotFound("actors.not.found", $"Actors with filter no found");
                _logger.LogWarning("Get filtered actors not found. SearchTerm={SearcTerm}, MovieId={MovieId}, Code={Code}",
                    actorFilter.SearchTerm, actorFilter.MovieId, actorsErorr.Code);
                return Result<(List<ActorListItemDto> Actors, int TotalCount)>.Fail(actorsErorr);
            }

            var totalCount = await _repository.CountFilteredAsync(actorFilter, cancellationToken);


            var actorsDto = _mapper.Map<List<ActorListItemDto>>(actors);

            return Result<(List<ActorListItemDto> Actors, int TotalCount)>.Success((actorsDto, totalCount));
        }

        public async Task<Result<ActorDetailsDto>> UpdateActorAsync(Guid targetId, ActorUpdateDto dto, CancellationToken cancellationToken)
        {
            var actor = await _repository.GetActorByIdAsync(targetId, cancellationToken);

            if (actor is null)
            {
                var actorErorr = Error.NotFound("actor.not.found", $"Actor with id: {targetId} not found");
                _logger.LogWarning("Update actor not found. ActorId={ActorId}, Code = {Code}", targetId, actorErorr.Code);
                return Result<ActorDetailsDto>.Fail(actorErorr);
            }


            _mapper.Map(dto, actor);

            _repository.UpdateActor(actor);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var updatedActorDto = _mapper.Map<ActorDetailsDto>(actor);

            return Result<ActorDetailsDto>.Success(updatedActorDto);
        }
    }
}
