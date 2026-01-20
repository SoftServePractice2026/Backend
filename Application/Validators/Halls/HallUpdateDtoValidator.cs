using Application.DTOs;
using FluentValidation;

namespace Application.Validators.Halls
{
    public class HallUpdateDtoValidator : AbstractValidator<HallUpdateDto>
    {
        public HallUpdateDtoValidator()
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
