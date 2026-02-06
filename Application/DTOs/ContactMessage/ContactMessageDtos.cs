using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.ContactMessage
{
    public record ContactMessageDto(
        string Name,
        string Email,
        string Message
        );
    

   
}
