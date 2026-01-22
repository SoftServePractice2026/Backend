using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Application.Services
{
    public interface IActorService
    {
        Task<Result<ActorDetailsDto>> CreateActorAsync(ActorCreateDto dto);
        Task<Result<ActorDetailsDto>> UpdateActorAsync(Guid targetId, ActorUpdateDto dto);
        Task<Result<bool>> DeleteActorAsync(Guid id);
        Task<Result<ActorDetailsDto>> GetActorByIdAsync(Guid id);
        Task<Result<List<ActorListItemDto>>> FindActorsByNameAsync(string name);
        Task<Result<List<ActorListItemDto>>> FindActorsBySurnameAsync(string surname);
        Task<Result<List<ActorListItemDto>>> FindActorsByFullNameAsync(string fullname);
        Task<Result<List<ActorListItemDto>>> GetActorAllAsync();
    }
    public class ActorService : IActorService
    {
        private readonly IActorRepository _repository;
        private readonly IMapper _mapper;

        public ActorService(IActorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<ActorDetailsDto>> CreateActorAsync(ActorCreateDto dto)
        {
            var actor = _mapper.Map<ActorEntity>(dto);
            try
            {
                await _repository.CreateActorAsync(actor);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Result<ActorDetailsDto>.Fail(
                    Failure.FromError(
                        Error.Internal(message: ex.Message)));
            }

            var resultDto = _mapper.Map<ActorDetailsDto>(actor);

            return Result<ActorDetailsDto>.Success(resultDto);
        }

        public async Task<Result<bool>> DeleteActorAsync(Guid id)
        {
            var actor = await _repository.GetActorByIdAsync(id);

            if (actor is null)
            {
                return Result<bool>.Fail(
                    Failure.FromError(
                        Error.NotFound("actor.not.found", $"Actor with id: {id} not found")));
            }

            await _repository.DeleteActorAsync(actor);
            await _repository.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<List<ActorListItemDto>>> FindActorsByFullNameAsync(string fullname)
        {
            var actors = await _repository.FindActorsBySurnameAsync(fullname);
            if (!actors.Any())
            {
                return Result<List<ActorListItemDto>>.Fail(
                    Failure.FromError(
                        Error.Conflict("actors.not.found", $"Actors with this full name: {fullname} not found")));
            }

            var actorsDto = _mapper.Map<List<ActorListItemDto>>(actors);

            return Result<List<ActorListItemDto>>.Success(actorsDto);
        }

        public async Task<Result<List<ActorListItemDto>>> FindActorsByNameAsync(string name)
        {
            var actors = await _repository.FindActorsByNameAsync(name);
            if (!actors.Any())
            {
                return Result<List<ActorListItemDto>>.Fail(
                    Failure.FromError(
                        Error.Conflict("actors.not.found", $"Actors with name: {name} not found")));
            }

            var actorsDto = _mapper.Map<List<ActorListItemDto>>(actors);

            return Result<List<ActorListItemDto>>.Success(actorsDto);
        }

        public async Task<Result<List<ActorListItemDto>>> FindActorsBySurnameAsync(string surname)
        {
            var actors = await _repository.FindActorsBySurnameAsync(surname);
            if (!actors.Any())
            {
                return Result<List<ActorListItemDto>>.Fail(
                    Failure.FromError(
                        Error.Conflict("actors.not.found", $"Actors with surname: {surname} not found")));
            }

            var actorsDto = _mapper.Map<List<ActorListItemDto>>(actors);

            return Result<List<ActorListItemDto>>.Success(actorsDto);
        }

        public async Task<Result<List<ActorListItemDto>>> GetActorAllAsync()
        {
            var actors = await _repository.GetActorEntitiesAsync();

            var actorsDto = _mapper.Map<List<ActorListItemDto>>(actors);

            return Result<List<ActorListItemDto>>.Success(actorsDto);
        }

        public async Task<Result<ActorDetailsDto>> GetActorByIdAsync(Guid id)
        {
            var actor = await _repository.GetActorByIdAsync(id);

            if (actor is null)
            {
                return Result<ActorDetailsDto>.Fail(
                    Failure.FromError(
                        Error.NotFound("actor.not.found", $"Actor with id: {id} not found")));
            }

            var actorDto = _mapper.Map<ActorDetailsDto>(actor);

            return Result<ActorDetailsDto>.Success(actorDto);
        }

        public async Task<Result<ActorDetailsDto>> UpdateActorAsync(Guid targetId, ActorUpdateDto dto)
        {
            var actor = await _repository.GetActorByIdAsync(targetId);

            if (actor is null)
            {
                return Result<ActorDetailsDto>.Fail(
                    Failure.FromError(
                        Error.NotFound("actor.not.found", $"Actor with id: {targetId} not found")));
            }


            _mapper.Map(dto, actor);

            await _repository.UpdateActorAsync(actor);
            await _repository.SaveChangesAsync();

            var updatedActorDto = _mapper.Map<ActorDetailsDto>(actor);

            return Result<ActorDetailsDto>.Success(updatedActorDto);
        }
    }
}
