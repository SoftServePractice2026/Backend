using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Shared;

namespace Application.Services
{
    public interface IViewHistoryService
    {
        Task<Result<ViewHistoryDetailsDto>> CreateViewHistoryAsync(ViewHistoryCreateDto dto);
        Task<Result<ViewHistoryDetailsDto>> UpdateViewHistoryAsync(Guid targetId, ViewHistoryUpdateDto dto);
        Task<Result<bool>> DeleteViewHistoryAsync(Guid id);
        Task<Result<ViewHistoryDetailsDto>> GetViewHistoryByIdAsync(Guid id);
        Task<Result<List<ViewHistoryListItemDto>>> GetViewHistoryAllAsync();
    }

    public class ViewHistoryService : IViewHistoryService
    {
        private readonly IViewHistoryRepository _repository;
        private readonly IMapper _mapper;

        public ViewHistoryService(IViewHistoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<ViewHistoryDetailsDto>> CreateViewHistoryAsync(ViewHistoryCreateDto dto)
        {
            var viewHistory = _mapper.Map<ViewHistoryEntity>(dto);

            try
            {
                await _repository.CreateViewHistoryAsync(viewHistory);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Result<ViewHistoryDetailsDto>.Fail(
                    Failure.FromError(Error.Internal(message: ex.Message)));
            }

            var resultDto = _mapper.Map<ViewHistoryDetailsDto>(viewHistory);
            return Result<ViewHistoryDetailsDto>.Success(resultDto);
        }

        public async Task<Result<ViewHistoryDetailsDto>> GetViewHistoryByIdAsync(Guid id)
        {
            var viewHistory = await _repository.GetViewHistoryByIdAsync(id);

            if (viewHistory is null)
            {
                return Result<ViewHistoryDetailsDto>.Fail(
                    Failure.FromError(Error.NotFound("viewHistory.not.found", $"View history with id: {id} not found")));
            }

            var viewHistoryDto = _mapper.Map<ViewHistoryDetailsDto>(viewHistory);
            return Result<ViewHistoryDetailsDto>.Success(viewHistoryDto);
        }

        public async Task<Result<List<ViewHistoryListItemDto>>> GetViewHistoryAllAsync()
        {
            var items = await _repository.GetViewHistoryEntitiesAsync();
            var itemsDto = _mapper.Map<List<ViewHistoryListItemDto>>(items);

            return Result<List<ViewHistoryListItemDto>>.Success(itemsDto);
        }

        public async Task<Result<ViewHistoryDetailsDto>> UpdateViewHistoryAsync(Guid targetId, ViewHistoryUpdateDto dto)
        {
            var viewHistory = await _repository.GetViewHistoryByIdAsync(targetId);

            if (viewHistory is null)
            {
                return Result<ViewHistoryDetailsDto>.Fail(
                    Failure.FromError(Error.NotFound("viewHistory.not.found", $"View history with id: {targetId} not found")));
            }

            _mapper.Map(dto, viewHistory);

            await _repository.UpdateViewHistoryAsync(viewHistory);
            await _repository.SaveChangesAsync();

            var updatedDto = _mapper.Map<ViewHistoryDetailsDto>(viewHistory);
            return Result<ViewHistoryDetailsDto>.Success(updatedDto);
        }

        public async Task<Result<bool>> DeleteViewHistoryAsync(Guid id)
        {
            var viewHistory = await _repository.GetViewHistoryByIdAsync(id);

            if (viewHistory is null)
            {
                return Result<bool>.Fail(
                    Failure.FromError(Error.NotFound("viewHistory.not.found", $"View history with id: {id} not found")));
            }

            await _repository.DeleteViewHistoryAsync(viewHistory);
            await _repository.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
