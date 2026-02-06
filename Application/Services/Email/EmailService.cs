using Application.DTOs.Actor;
using Application.DTOs.Common;
using Application.DTOs.ContactMessage;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Shared;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading;

namespace Application.Services.Email
{
    public class EmailService : IEmailService
    {
        public async Task<Result<bool>> SendEmailAsync(ContactMessageDto contact, CancellationToken ct)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse(contact.Email));
                message.To.Add(MailboxAddress.Parse("cinemadmin@gmail.com"));
                message.Subject = $"User: {contact.Name}";
                message.Body = new TextPart(TextFormat.Plain)
                {
                    Text = contact.Message
                };

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                await smtp.ConnectAsync("localhost", 1025, SecureSocketOptions.None, ct);
                await smtp.SendAsync(message, ct);
                await smtp.DisconnectAsync(true, ct);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                var error = Error.Failure("email.send.failed", ex.Message);

                return Result<bool>.Fail(error.ToFailure());
            }
        }
    }
}
