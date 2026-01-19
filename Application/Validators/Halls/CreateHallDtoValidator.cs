using Application.DTOs;
using FluentValidation;

namespace Application.Validators.Halls
{
    public class CreateHallDtoValidator : AbstractValidator<CreateHallDto>
    {
        public CreateHallDtoValidator()
        {
            RuleFor(h => h.Name)
                .NotEmpty().WithMessage("Hall name is required").WithErrorCode("name.empty")
                .MinimumLength(1)
                .MaximumLength(10);

            RuleFor(h => h.HallSize)
                .IsInEnum().WithMessage("Invalid hall size").WithErrorCode("hallSize.invalid");
        }
    }
}
