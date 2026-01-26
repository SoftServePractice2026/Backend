using Application.DTOs.Genre;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public interface IGenreService
    {
        Task<Result<GenreDetailsDto>> CreateGenreAsync(GenreCreateDto dto);
        Task<Result<GenreDetailsDto>> UpdateGenreAsync(Guid targetId, GenreUpdateDto dto);
        Task<Result<bool>> DeleteGenreAsync(Guid id);
        Task<Result<GenreDetailsDto>> GetGenreByIdAsync(Guid id);
        Task<Result<GenreDetailsDto>> GetGenreByNameAsync(string name);
        Task<Result<List<GenreListItemDto>>> GetGenreAllAsync();
    }

    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _repository;
        private readonly IMapper _mapper;

        public GenreService(IGenreRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<GenreDetailsDto>> CreateGenreAsync(GenreCreateDto dto)
        {
            var exist = await _repository.GetGenreByNameAsync(dto.Name);
            if (exist is not null)
            {
                return Result<GenreDetailsDto>.Fail(
                    Failure.FromError(
                        Error.Conflict("genre.exist", $"Genre with name: {dto.Name} already exist")));
            }

            var genre = _mapper.Map<GenreEntity>(dto);

            try
            {
                await _repository.CreateGenreAsync(genre);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Result<GenreDetailsDto>.Fail(
                    Failure.FromError(
                        Error.Internal(message: ex.Message)));
            }

            var resultDto = _mapper.Map<GenreDetailsDto>(genre);

            return Result<GenreDetailsDto>.Success(resultDto);
        }

        public async Task<Result<bool>> DeleteGenreAsync(Guid id)
        {
            var genre = await _repository.GetGenreByIdAsync(id);

            if (genre is null)
            {
                return Result<bool>.Fail(
                    Failure.FromError(
                        Error.NotFound("genre.not.found", $"Genre with id: {id} not found")));
            }

            await _repository.DeleteGenreAsync(genre);
            await _repository.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<List<GenreListItemDto>>> GetGenreAllAsync()
        {
            var genres = await _repository.GetGenreEntitiesAsync();

            var genresDto = _mapper.Map<List<GenreListItemDto>>(genres);

            return Result<List<GenreListItemDto>>.Success(genresDto);
        }

        public async Task<Result<GenreDetailsDto>> GetGenreByIdAsync(Guid id)
        {
            var genre = await _repository.GetGenreByIdAsync(id);

            if (genre is null)
            {
                return Result<GenreDetailsDto>.Fail(
                    Failure.FromError(
                        Error.NotFound("genre.not.found", $"Genre with id: {id} not found")));
            }

            var genreDto = _mapper.Map<GenreDetailsDto>(genre);

            return Result<GenreDetailsDto>.Success(genreDto);
        }

        public async Task<Result<GenreDetailsDto>> GetGenreByNameAsync(string name)
        {
            var genre = await _repository.GetGenreByNameAsync(name);
            if (genre is null)
            {
                return Result<GenreDetailsDto>.Fail(
                    Failure.FromError(
                        Error.Conflict("genre.not.found", $"Genre with name: {name} not found")));
            }

            var genreDto = _mapper.Map<GenreDetailsDto>(genre);

            return Result<GenreDetailsDto>.Success(genreDto);
        }

        public async Task<Result<GenreDetailsDto>> UpdateGenreAsync(Guid targetId, GenreUpdateDto dto)
        {
            var genre = await _repository.GetGenreByIdAsync(targetId);

            if (genre is null)
            {
                return Result<GenreDetailsDto>.Fail(
                    Failure.FromError(
                        Error.NotFound("genre.not.found", $"Genre with id: {targetId} not found")));
            }

            var nameExist = await _repository.GetGenreByNameAsync(dto.Name);
            if (nameExist is not null && nameExist.Id != targetId)
            {
                return Result<GenreDetailsDto>.Fail(
                    Failure.FromError(
                        Error.Conflict("genre.exist", $"Genre with name: {dto.Name} already exist")));
            }

            _mapper.Map(dto, genre);

            await _repository.UpdateGenreAsync(genre);
            await _repository.SaveChangesAsync();

            var updatedGenreDto = _mapper.Map<GenreDetailsDto>(genre);

            return Result<GenreDetailsDto>.Success(updatedGenreDto);
        }
    }
}
