namespace Application.DTOs.Email
{
    public record EmailSendMessageRequest(
        string SenderEmail,
        string RecieverEmail,
        string Header,
        string Message);

    public record ContactMessageDto(
        string Name,
        string Email,
        string Message
        );
}
