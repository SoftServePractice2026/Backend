using Application.DTOs;
using FluentValidation;

namespace Application.Validators.Tickets
{
    public class TicketCreateDtoValidator : AbstractValidator<TicketCreateDto>
    {
        public TicketCreateDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required").WithErrorCode("userId.empty");

            RuleFor(x => x.SeatId)
                .NotEmpty().WithMessage("Seat ID is required").WithErrorCode("seatId.empty");

            RuleFor(x => x.SessionId)
                .NotEmpty().WithMessage("Session ID is required").WithErrorCode("sessionId.empty");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0").WithErrorCode("price.invalid");

            RuleFor(x => x.TicketStatus)
                .IsInEnum().WithMessage("Invalid ticket status").WithErrorCode("ticketStatus.invalid");
        }
    }
}
