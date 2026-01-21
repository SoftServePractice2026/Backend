using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Shared;

namespace Application.Services
{
    public interface IHallService
    {
        Task<Result<HallDetailsDto>> CreateHallAsync(HallCreateDto dto);
        Task<Result<HallDetailsDto>> UpdateHallAsync(Guid targetId, HallUpdateDto dto);
        Task<Result<bool>> DeleteHallAsync(Guid id);
        Task<Result<HallDetailsDto>> GetHallByIdAsync(Guid id);
        Task<Result<HallDetailsDto>> GetHallByNameAsync(string name);
        Task<Result<List<HallListItemDto>>> GetHallAllAsync();
    }

    public class HallService : IHallService
    {
        private readonly IHallRepository _repository;
        private readonly IMapper _mapper;

        public HallService(IHallRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<HallDetailsDto>> CreateHallAsync(HallCreateDto dto)
        {
            var exist = await _repository.GetHallByNameAsync(dto.Name);
            if (exist is not null)
            {
                return Result<HallDetailsDto>.Fail(
                    Failure.FromError(
                        Error.Conflict("hall.exist", $"Hall with name: {dto.Name} already exist")));
            }

            var hall = _mapper.Map<HallEntity>(dto);

            try
            {
                await _repository.CreateHallAsync(hall);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Result<HallDetailsDto>.Fail(
                    Failure.FromError(
                        Error.Internal(message: ex.Message)));
            }

            var resultDto = _mapper.Map<HallDetailsDto>(hall);

            return Result<HallDetailsDto>.Success(resultDto);
        }

        public async Task<Result<bool>> DeleteHallAsync(Guid id)
        {
            var hall = await _repository.GetHallByIdAsync(id);

            if (hall is null)
            {
                return Result<bool>.Fail(
                    Failure.FromError(
                        Error.NotFound("hall.not.found", $"Hall with id: {id} not found")));
            }

            await _repository.DeleteHallAsync(hall);
            await _repository.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<List<HallListItemDto>>> GetHallAllAsync()
        {
            var halls = await _repository.GetHallEntitiesAsync();

            var hallsDto = _mapper.Map<List<HallListItemDto>>(halls);

            return Result<List<HallListItemDto>>.Success(hallsDto);
        }

        public async Task<Result<HallDetailsDto>> GetHallByIdAsync(Guid id)
        {
            var hall = await _repository.GetHallByIdAsync(id);

            if (hall is null)
            {
                return Result<HallDetailsDto>.Fail(
                    Failure.FromError(
                        Error.NotFound("hall.not.found", $"Hall with id: {id} not found")));
            }

            var hallDto = _mapper.Map<HallDetailsDto>(hall);

            return Result<HallDetailsDto>.Success(hallDto);
        }

        public async Task<Result<HallDetailsDto>> GetHallByNameAsync(string name)
        {
            var hall = await _repository.GetHallByNameAsync(name);
            if (hall is null)
            {
                return Result<HallDetailsDto>.Fail(
                    Failure.FromError(
                        Error.Conflict("hall.not.found", $"Hall with name: {name} not found")));
            }

            var hallDto = _mapper.Map<HallDetailsDto>(hall);

            return Result<HallDetailsDto>.Success(hallDto);
        }

        public async Task<Result<HallDetailsDto>> UpdateHallAsync(Guid targetId, HallUpdateDto dto)
        {
            var hall = await _repository.GetHallByIdAsync(targetId);

            if (hall is null)
            {
                return Result<HallDetailsDto>.Fail(
                    Failure.FromError(
                        Error.NotFound("hall.not.found", $"Hall with id: {targetId} not found")));
            }

            var nameExist = await _repository.GetHallByNameAsync(dto.Name);
            if (nameExist is not null && nameExist.Id != targetId)
            {
                return Result<HallDetailsDto>.Fail(
                    Failure.FromError(
                        Error.Conflict("hall.exist", $"Hall with name: {dto.Name} already exist")));
            }

            _mapper.Map(dto, hall);

            await _repository.UpdateHallAsync(hall);
            await _repository.SaveChangesAsync();

            var updatedHallDto = _mapper.Map<HallDetailsDto>(hall);

            return Result<HallDetailsDto>.Success(updatedHallDto);
        }
    }
}
