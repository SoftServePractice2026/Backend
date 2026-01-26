using Application.DTOs.Actor;
using AutoMapper;
using Domain.Entities;
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

        public Task<Result<ActorDetailsDto>> CreateActorAsync(ActorCreateDto dto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> DeleteActorAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ActorDetailsDto>> GetActorByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<(List<ActorListItemDto> Actors, int TotalCount)>> GetFilteredHallsAsync(ActorFilterDto actorFilterDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ActorDetailsDto>> UpdateActorAsync(Guid targetId, ActorUpdateDto dto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
