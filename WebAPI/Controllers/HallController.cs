using Application.DTOs;
using Application.Services;
using Application.Services.Hall;
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
        public async Task<IActionResult> Create([FromBody] HallCreateDto dto, CancellationToken cancellationToken)
        {
            var result = await _hallService.CreateHallAsync(dto, cancellationToken);

            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : Ok(result.Value);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, HallUpdateDto dto, CancellationToken cancellation)
        {
            var result = await _hallService.UpdateHallAsync(id, dto, cancellation);
            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : Ok(result.Value);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _hallService.DeleteHallAsync(id, cancellationToken);
            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : NoContent();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _hallService.GetHallByIdAsync(id, cancellationToken);
            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : Ok(result.Value);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name, CancellationToken cancellationToken)
        {
            var result = await _hallService.GetHallByNameAsync(name, cancellationToken);
            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _hallService.GetHallsAsync(cancellationToken);
            return Ok(result.Value);
        }
    }
}
