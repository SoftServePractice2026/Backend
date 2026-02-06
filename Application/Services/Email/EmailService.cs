using Application.DTOs.ContactMessage;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Application.Services.Email
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(ContactMessageDto contact)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(contact.Email));
            message.To.Add(MailboxAddress.Parse("cinemadmin@gmail.com"));
            message.Subject = $"User: {contact.Name}";
            message.Body = new TextPart(TextFormat.Plain) { Text = $"""{contact.Message}"""};


            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync("localhost", 8025);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}
