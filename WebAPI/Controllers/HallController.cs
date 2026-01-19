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
        public async Task<IActionResult> Create([FromBody] CreateHallDto dto)
        {
            var result = await _hallService.CreateHallAsync(dto);

            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : Ok(result.Value);
        }
    }
}
