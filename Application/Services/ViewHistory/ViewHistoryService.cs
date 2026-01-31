using Application.DTOs;
using Application.Services.ViewHistory;
using AutoMapper;
using Domain.Entities;
using Domain.Filters;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Services
{
    public class ViewHistoryService : IViewHistoryService
    {
        private readonly IViewHistoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ViewHistoryService> _logger;

        public ViewHistoryService(
            IViewHistoryRepository repository, 
            IMapper mapper, 
            IUnitOfWork unitOfWork, 
            ILogger<ViewHistoryService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
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
                _logger.LogWarning("Delete view history not found. HistoryId={HistoryId}, Code = {Code}", id, error.Code);
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
                _logger.LogWarning("Get view history by id not found. HistoryId={HistoryId}, Code = {Code}", id, error.Code);
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
                _logger.LogWarning("Update view history not found. HistoryId={HistoryId}, Code = {Code}", targetId, error.Code);
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
                _logger.LogWarning("Get filtered view history not found. Code={Code}", error.Code);
                return Result<(List<ViewHistoryListItemDto> Items, int TotalCount)>.Fail(error);
            }

            var totalCount = await _repository.CountFilteredAsync(filter, cancellationToken);

            var dtos = _mapper.Map<List<ViewHistoryListItemDto>>(items);

            return Result<(List<ViewHistoryListItemDto> Items, int TotalCount)>.Success((dtos, totalCount));
        }
        
        public async Task<Result<List<ViewHistoryListItemDto>>> GetViewHistoryByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request started: get view history for user {UserId}", userId);
            
            var filter = new ViewHistoryFilter 
            { 
                UserId = userId,
                PageSize = 100
            };
            
            var items = await _repository.GetFilteredViewHistoryAsync(filter, cancellationToken);

            if (items == null || !items.Any())
            {
                var error = Error.NotFound("viewHistory.user.empty", $"No view history found for user with id: {userId}");
                _logger.LogWarning("Get view history for user failed. UserId={UserId}, Code={Code}", userId, error.Code);
                return Result<List<ViewHistoryListItemDto>>.Fail(error);
            }

            var dtos = _mapper.Map<List<ViewHistoryListItemDto>>(items);

            _logger.LogInformation("Request ended: get view history for user {UserId}. Found {Count} items", userId, dtos.Count);
            return Result<List<ViewHistoryListItemDto>>.Success(dtos);
        }
    }
}
