using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Mappers;

namespace WebAPI.Controllers
{
    [Route("api/ticket")]
    public class TicketController : BaseController
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TicketCreateDto dto)
        {
            var result = await _ticketService.CreateTicketAsync(dto);

            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : Ok(result.Value);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TicketUpdateDto dto)
        {
            var result = await _ticketService.UpdateTicketAsync(id, dto);
            
            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : Ok(result.Value);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _ticketService.DeleteTicketAsync(id);
            
            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : NoContent();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _ticketService.GetTicketByIdAsync(id);
            
            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _ticketService.GetTicketAllAsync();
            
            return result.IsFailure
                ? FailureMapper.ToHttp(result.Failure!)
                : Ok(result.Value);
        }
    }
}