using Application.DTOs.ContactMessage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(ContactMessageDto contact);
    }
}
