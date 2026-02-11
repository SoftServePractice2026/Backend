using Application.DTOs.Email;
using Shared;

namespace Application.Services.Email
{
    public interface IEmailService
    {
        Task<Result<bool>> SendEmailAsync(ContactMessageDto contact, CancellationToken ct);
        Task<Result<bool>> SendEmailAsync(EmailSendMessageRequest request, CancellationToken ct);
    }
}
