using Application.DTOs.ContactMessage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.Email
{
    public class EmailService : IEmailService
    {
        public Task SendEmailAsync(ContactMessageDto contact)
        {
            throw new NotImplementedException();
        }
    }
}
