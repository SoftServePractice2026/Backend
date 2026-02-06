using Application.DTOs.ContactMessage;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation.Validators;

namespace Application.Validators.Emails
{
    public class ContactMessageDtoValidator : AbstractValidator<ContactMessageDto>
    {
        public ContactMessageDtoValidator() 
        {
            RuleFor(n => n.Name)
                .NotEmpty().WithMessage("User name is required")
                .WithErrorCode("name.empty")
                .MinimumLength(1)
                .MaximumLength(50);

            RuleFor(n => n.Message)
                .NotEmpty().WithMessage("Message text is required")
                .WithErrorCode("message.empty")
                .MinimumLength(1)
                .MaximumLength(2000);

            RuleFor(n => n.Email)
                .NotEmpty().WithMessage("Email must not be empty")
                .EmailAddress();
        }
    }
}
