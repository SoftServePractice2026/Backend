using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Shared;

namespace Application.Services
{
    public interface IHallService
    {
        Task<Result<HallDetailsDto>> CreateHallAsync(CreateHallDto dto);
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

        public async Task<Result<HallDetailsDto>> CreateHallAsync(CreateHallDto dto)
        {
            // Перевірка на дуплікат

            var hall = _mapper.Map<HallEntity>(dto);

            try
            {
                await _repository.CreateHallAsync(hall);
            }
            catch
            {
                return Result<HallDetailsDto>.Fail(Error.Internal());
            }

            var resultDto = _mapper.Map<HallDetailsDto>(hall);

            return Result<HallDetailsDto>.Success(resultDto);
        }
    }
}
