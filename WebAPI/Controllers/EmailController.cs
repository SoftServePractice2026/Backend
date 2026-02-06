using Application.DTOs.Actor;
using Application.DTOs.ContactMessage;
using Application.Services.Email;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Shared;
using WebAPI.ResponseExtensions;

namespace WebAPI.Controllers
{
    [Route("api/v1/emails")]
    public class EmailController : BaseController
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ContactMessageDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ContactMessageDto dto, CancellationToken ct)
        {
            var result = await _emailService.SendEmailAsync(dto, ct);

            if (result.IsFailure)
            {
                return result.Failure!.ToResponse();
            }

            return Ok(new { message = "Повідомлення успішно відправлено!"});
        }
    }
}
