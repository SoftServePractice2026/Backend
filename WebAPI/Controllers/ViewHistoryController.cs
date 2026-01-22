using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Mappers;

namespace WebAPI.Controllers
{
    public class ViewHistoryController : BaseController
    {
        private readonly IViewHistoryService _viewHistoryService;

        public ViewHistoryController(IViewHistoryService viewHistoryService)
        {
            _viewHistoryService = viewHistoryService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ViewHistoryCreateDto dto)
        {
            var result = await _viewHistoryService.CreateViewHistoryAsync(dto);

            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : Ok(result.Value);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ViewHistoryUpdateDto dto)
        {
            var result = await _viewHistoryService.UpdateViewHistoryAsync(id, dto);
            
            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : Ok(result.Value);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _viewHistoryService.DeleteViewHistoryAsync(id);
            
            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : NoContent();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _viewHistoryService.GetViewHistoryByIdAsync(id);
            
            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _viewHistoryService.GetViewHistoryAllAsync();
            
            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : Ok(result.Value);
        }
    }
}