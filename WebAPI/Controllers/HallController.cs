using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Mappers;

namespace WebAPI.Controllers
{
    public class HallController : BaseController
    {
        private readonly IHallService _hallService;

        public HallController(IHallService hallService)
        {
            _hallService = hallService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HallCreateDto dto)
        {
            var result = await _hallService.CreateHallAsync(dto);

            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : Ok(result.Value);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, HallUpdateDto dto)
        {
            var result = await _hallService.UpdateHallAsync(id, dto);
            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : Ok(result.Value);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _hallService.DeleteHallAsync(id);
            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : NoContent();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _hallService.GetHallByIdAsync(id);
            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : Ok(result.Value);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _hallService.GetHallByNameAsync(name);
            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _hallService.GetHallAllAsync();
            return Ok(result.Value);
        }
    }
}
