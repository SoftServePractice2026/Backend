using Application.DTOs.Seat.SeatCRUD;
using FluentValidation;

namespace Application.Validators.Seats;

public class UpdateSeatValidator: AbstractValidator<UpdateSeatDto>
{
    public UpdateSeatValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Seat ID is required");

        RuleFor(x => x.RowNumber)
            .GreaterThan(0)
            .WithMessage("Row number must be greater than 0")
            .LessThanOrEqualTo(50)
            .WithMessage("Row number must not exceed 50");

        RuleFor(x => x.SeatNumber)
            .GreaterThan(0)
            .WithMessage("Seat number must be greater than 0")
            .LessThanOrEqualTo(100)
            .WithMessage("Seat number must not exceed 100");

        RuleFor(x => x.SeatType)
            .IsInEnum()
            .WithMessage("Invalid seat type");

        RuleFor(x => x.SeatStatus)
            .IsInEnum()
            .WithMessage("Invalid seat status");
    }
}