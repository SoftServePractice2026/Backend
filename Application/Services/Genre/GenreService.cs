using Application.DTOs;
using Application.DTOs.Genre;
using AutoMapper;
using Domain.Entities;
using Domain.Filters;
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

        public async Task<Result<bool>> DeleteGenreAsync(Guid id, CancellationToken cancellationToken)
        {
            var genre = await _repository.GetGenreByIdAsync(id, cancellationToken);

            if (genre is null)
            {
                var genreErorr = Error.NotFound("genre.not.found", $"Genre with id: {id} not found");
                _logger.LogWarning("Delete genre not found. GenreId={GenreId}, Code = {Code}", id, genreErorr.Code);
                return Result<bool>.Fail(genreErorr);
            }

            _repository.DeleteGenre(genre);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<List<GenreListItemDto>>> GetGenreAllAsync(
    GenreFilterDto filter,
    CancellationToken cancellationToken)
        {
            var genres = await _repository.GetAllGenresAsync(filter, cancellationToken);

            if (genres == null || !genres.Any())
            {
                var error = Error.NotFound("genres.not.found", "Genres not found");

                _logger.LogWarning("Get genres failed. Code={Code}", error.Code);

                return Result<List<GenreListItemDto>>.Fail(error);
            }

            var resultDto = _mapper.Map<List<GenreListItemDto>>(genres);

            return Result<List<GenreListItemDto>>.Success(resultDto);
        }

        public async Task<Result<GenreDetailsDto>> GetGenreByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var genre = await _repository.GetGenreByIdAsync(id, cancellationToken);

            if (genre is null)
            {
                var genreErorr = Error.NotFound("genre.not.found", $"Genre with id: {id} not found");
                _logger.LogWarning("Get genre by id not found. HallId={HallId}, Code = {Code}", id, genreErorr.Code);
                return Result<GenreDetailsDto>.Fail(genreErorr);
            }

            var genreDto = _mapper.Map<GenreDetailsDto>(genre);

            return Result<GenreDetailsDto>.Success(genreDto);
        }

        public async Task<Result<GenreDetailsDto>> GetGenreByNameAsync(string name, CancellationToken cancellationToken)
        {
            var genre = await _repository.GetGenreByNameAsync(name, cancellationToken);

            if (genre is null)
            {
                var genreErorr = Error.NotFound("genre.not.found", $"Genre with name: {name} not found");
                _logger.LogWarning("Get genre by name not found. GenreName={GenreName}, Code = {Code}", name, genreErorr.Code);
                return Result<GenreDetailsDto>.Fail(genreErorr);
            }

            var genreDto = _mapper.Map<GenreDetailsDto>(genre);

            return Result<GenreDetailsDto>.Success(genreDto);
        }

        public async Task<Result<GenreDetailsDto>> UpdateGenreAsync(Guid targetId, GenreUpdateDto dto, CancellationToken cancellationToken)
        {
            var genre = await _repository.GetGenreByIdAsync(targetId, cancellationToken);

            if (genre is null)
            {
                var genreErorr = Error.NotFound("genre.not.found", $"Genre with id: {targetId} not found");
                _logger.LogWarning("Update genre not found. GenreId={GenreId}, Code = {Code}", targetId, genreErorr.Code);
                return Result<GenreDetailsDto>.Fail(genreErorr);
            }

            var nameExist = await _repository.GetGenreByNameAsync(dto.Name, cancellationToken);

            if (nameExist is not null && nameExist.Id != targetId)
            {
                var nameExistErorr = Error.Conflict("genre.exist", $"Genre with name: {dto.Name} already exist");
                _logger.LogWarning("Update genre exist. GenreName={GenreName}, Code = {Code}", dto.Name, nameExistErorr.Code);
                return Result<GenreDetailsDto>.Fail(nameExistErorr);
            }

            _mapper.Map(dto, genre);

            _repository.UpdateGenre(genre);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var updatedGenreDto = _mapper.Map<GenreDetailsDto>(genre);

            return Result<GenreDetailsDto>.Success(updatedGenreDto);
        }
    }
}
