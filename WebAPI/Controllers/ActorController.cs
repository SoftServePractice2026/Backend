using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Mappers;

namespace WebAPI.Controllers
{
    public class ActorController : BaseController
    {
        private readonly IActorService _actorService;

        public ActorController(IActorService actorService)
        {
            _actorService = actorService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ActorCreateDto dto)
        {
            var result = await _actorService.CreateActorAsync(dto);

            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : Ok(result.Value);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, ActorUpdateDto dto)
        {
            var result = await _actorService.UpdateActorAsync(id, dto);
            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : Ok(result.Value);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _actorService.DeleteActorAsync(id);
            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : NoContent();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _actorService.GetActorByIdAsync(id);
            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : Ok(result.Value);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _actorService.FindActorsByNameAsync(name);
            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : Ok(result.Value);
        }

        [HttpGet("{surname}")]
        public async Task<IActionResult> GetBySurname(string surname)
        {
            var result = await _actorService.FindActorsBySurnameAsync(surname);
            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : Ok(result.Value);
        }

        [HttpGet("{fullname}")]
        public async Task<IActionResult> GetByFullname(string fullname)
        {
            var result = await _actorService.FindActorsByFullNameAsync(fullname);
            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _actorService.GetActorAllAsync();
            return Ok(result.Value);
        }
    }
}
