using Application.DTOs.Actor;
using Application.DTOs.ContactMessage;
using System;
using System.Collections.Generic;
using System.Text;
using Shared;

namespace Application.Services.Email
{
    public interface IEmailService
    {
        Task<Result<bool>> SendEmailAsync(ContactMessageDto contact, CancellationToken ct);
    }
}
