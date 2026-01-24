using Application.DTOs;
using Application.Services.Hall;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Shared;

namespace Application.Services
{
    public class HallService : IHallService
    {
        private readonly IHallRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public HallService(IHallRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<HallDetailsDto>> CreateHallAsync(HallCreateDto dto, CancellationToken cancellationToken)
        {
            var exist = await _repository.GetHallByNameAsync(dto.Name, cancellationToken);
            if (exist is not null)
            {
                return Result<HallDetailsDto>.Fail(
                    Failure.FromError(
                        Error.Conflict("hall.exist", $"Hall with name: {dto.Name} already exist")));
            }

            var hall = _mapper.Map<HallEntity>(dto);

            try
            {
                _repository.CreateHall(hall);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
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

        public async Task<Result<bool>> DeleteHallAsync(Guid id, CancellationToken cancellationToken)
        {
            var hall = await _repository.GetHallByIdAsync(id, cancellationToken);

            if (hall is null)
            {
                return Result<bool>.Fail(
                    Failure.FromError(
                        Error.NotFound("hall.not.found", $"Hall with id: {id} not found")));
            }

            _repository.DeleteHall(hall);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<List<HallListItemDto>>> GetHallsAsync(CancellationToken cancellationToken)
        {
            var halls = await _repository.GetHallsAsync(cancellationToken);

            var hallsDto = _mapper.Map<List<HallListItemDto>>(halls);

            return Result<List<HallListItemDto>>.Success(hallsDto);
        }

        public async Task<Result<HallDetailsDto>> GetHallByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var hall = await _repository.GetHallByIdAsync(id, cancellationToken);

            if (hall is null)
            {
                return Result<HallDetailsDto>.Fail(
                    Failure.FromError(
                        Error.NotFound("hall.not.found", $"Hall with id: {id} not found")));
            }

            var hallDto = _mapper.Map<HallDetailsDto>(hall);

            return Result<HallDetailsDto>.Success(hallDto);
        }

        public async Task<Result<HallDetailsDto>> GetHallByNameAsync(string name, CancellationToken cancellationToken)
        {
            var hall = await _repository.GetHallByNameAsync(name, cancellationToken);
            if (hall is null)
            {
                return Result<HallDetailsDto>.Fail(
                    Failure.FromError(
                        Error.Conflict("hall.not.found", $"Hall with name: {name} not found")));
            }

            var hallDto = _mapper.Map<HallDetailsDto>(hall);

            return Result<HallDetailsDto>.Success(hallDto);
        }

        public async Task<Result<HallDetailsDto>> UpdateHallAsync(Guid targetId, HallUpdateDto dto, CancellationToken cancellationToken)
        {
            var hall = await _repository.GetHallByIdAsync(targetId, cancellationToken);

            if (hall is null)
            {
                return Result<HallDetailsDto>.Fail(
                    Failure.FromError(
                        Error.NotFound("hall.not.found", $"Hall with id: {targetId} not found")));
            }

            var nameExist = await _repository.GetHallByNameAsync(dto.Name, cancellationToken);
            if (nameExist is not null && nameExist.Id != targetId)
            {
                return Result<HallDetailsDto>.Fail(
                    Failure.FromError(
                        Error.Conflict("hall.exist", $"Hall with name: {dto.Name} already exist")));
            }

            _mapper.Map(dto, hall);

            _repository.UpdateHall(hall);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var updatedHallDto = _mapper.Map<HallDetailsDto>(hall);

            return Result<HallDetailsDto>.Success(updatedHallDto);
        }
    }
}
