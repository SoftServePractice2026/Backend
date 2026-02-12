using Application.DTOs.Email;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Shared;

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

        public async Task<Result<bool>> SendEmailAsync(EmailSendMessageRequest request, CancellationToken ct)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse(request.SenderEmail));
                message.To.Add(MailboxAddress.Parse(request.RecieverEmail));
                message.Subject = request.Header;
                message.Body = new TextPart(TextFormat.Plain)
                {
                    Text = request.Message
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
