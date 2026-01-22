using Application.DTOs;
using FluentValidation;

namespace Application.Validators.Tickets
{
    public class TicketUpdateDtoValidator : AbstractValidator<TicketUpdateDto>
    {
        public TicketUpdateDtoValidator()
        {
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0").WithErrorCode("price.invalid");

            RuleFor(x => x.TicketStatus)
                .IsInEnum().WithMessage("Invalid ticket status").WithErrorCode("ticketStatus.invalid");
        }
    }
}