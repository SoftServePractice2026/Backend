using Application.DTOs;
using Application.DTOs.Genre;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.Genre
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GenreService> _logger;

        public GenreService(IGenreRepository repository, IMapper mapper, IUnitOfWork unitOfWork, ILogger<GenreService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<GenreDetailsDto>> CreateGenreAsync(GenreCreateDto dto, CancellationToken cancellationToken)
        {
            var exist = await _repository.GetGenreByNameAsync(dto.Name, cancellationToken);

            if (exist is not null)
            {
                var existErorr = Error.Conflict("genre.exist", $"Genre with name: {dto.Name} already exist");
                _logger.LogWarning("Create genre conflict. GenreName={GenreName}, Code = {Code}", dto.Name, existErorr.Code);
                return Result<GenreDetailsDto>.Fail(existErorr);
            }

            var genre = _mapper.Map<GenreEntity>(dto);

            _repository.CreateGenre(genre);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = _mapper.Map<GenreDetailsDto>(genre);

            return Result<GenreDetailsDto>.Success(resultDto);
        }

        public Task<Result<bool>> DeleteGenreAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<GenreListItemDto>>> GetGenreAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<GenreDetailsDto>> GetGenreByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<GenreDetailsDto>> GetGenreByNameAsync(string name, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<GenreDetailsDto>> UpdateGenreAsync(Guid targetId, GenreUpdateDto dto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
