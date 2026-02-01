using Application.DTOs;
using Application.Services.ViewHistory;
using AutoMapper;
using Domain.Entities;
using Domain.Filters;
using Domain.Interfaces;
using Shared;

namespace Application.Services
{
    public class ViewHistoryService : IViewHistoryService
    {
        private readonly IViewHistoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ViewHistoryService(
            IViewHistoryRepository repository, 
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ViewHistoryDetailsDto>> CreateViewHistoryAsync(ViewHistoryCreateDto dto, CancellationToken cancellationToken)
        {
            
            var viewHistory = _mapper.Map<ViewHistoryEntity>(dto);

            _repository.CreateViewHistory(viewHistory);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = _mapper.Map<ViewHistoryDetailsDto>(viewHistory);

            return Result<ViewHistoryDetailsDto>.Success(resultDto);
        }

        public async Task<Result<bool>> DeleteViewHistoryAsync(Guid id, CancellationToken cancellationToken)
        {
            var viewHistory = await _repository.GetViewHistoryByIdAsync(id, cancellationToken);

            if (viewHistory is null)
            {
                var error = Error.NotFound("viewHistory.not.found", $"View history with id: {id} not found");
                return Result<bool>.Fail(error);
            }

            _repository.DeleteViewHistory(viewHistory);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }

        public async Task<Result<ViewHistoryDetailsDto>> GetViewHistoryByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var viewHistory = await _repository.GetViewHistoryByIdAsync(id, cancellationToken);

            if (viewHistory is null)
            {
                var error = Error.NotFound("viewHistory.not.found", $"View history with id: {id} not found");
                return Result<ViewHistoryDetailsDto>.Fail(error);
            }

            var dto = _mapper.Map<ViewHistoryDetailsDto>(viewHistory);

            return Result<ViewHistoryDetailsDto>.Success(dto);
        }

        public async Task<Result<ViewHistoryDetailsDto>> UpdateViewHistoryAsync(Guid targetId, ViewHistoryUpdateDto dto, CancellationToken cancellationToken)
        {
            var viewHistory = await _repository.GetViewHistoryByIdAsync(targetId, cancellationToken);

            if (viewHistory is null)
            {
                var error = Error.NotFound("viewHistory.not.found", $"View history with id: {targetId} not found");
                return Result<ViewHistoryDetailsDto>.Fail(error);
            }

            _mapper.Map(dto, viewHistory);

            _repository.UpdateViewHistory(viewHistory);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var updatedDto = _mapper.Map<ViewHistoryDetailsDto>(viewHistory);

            return Result<ViewHistoryDetailsDto>.Success(updatedDto);
        }

        public async Task<Result<(List<ViewHistoryListItemDto> Items, int TotalCount)>> GetFilteredViewHistoryAsync(ViewHistoryFilterDto filterDto, CancellationToken cancellationToken)
        {
            var filter = _mapper.Map<ViewHistoryFilter>(filterDto);

            var items = await _repository.GetFilteredViewHistoryAsync(filter, cancellationToken);

            if (!items.Any())
            {
                var error = Error.NotFound("viewHistory.not.found", "View history items with filter not found");
                return Result<(List<ViewHistoryListItemDto> Items, int TotalCount)>.Fail(error);
            }

            var totalCount = await _repository.CountFilteredAsync(filter, cancellationToken);

            var dtos = _mapper.Map<List<ViewHistoryListItemDto>>(items);

            return Result<(List<ViewHistoryListItemDto> Items, int TotalCount)>.Success((dtos, totalCount));
        }
        
        public async Task<Result<List<ViewHistoryListItemDto>>> GetViewHistoryByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var filter = new ViewHistoryFilter 
            { 
                UserId = userId,
                PageSize = 100
            };
            
            var items = await _repository.GetFilteredViewHistoryAsync(filter, cancellationToken);

            if (items == null || !items.Any())
            {
                var error = Error.NotFound("viewHistory.user.empty", $"No view history found for user with id: {userId}");
                return Result<List<ViewHistoryListItemDto>>.Fail(error);
            }

            var dtos = _mapper.Map<List<ViewHistoryListItemDto>>(items);

            return Result<List<ViewHistoryListItemDto>>.Success(dtos);
        }
    }
}
