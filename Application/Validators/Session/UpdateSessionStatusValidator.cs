using Application.DTOs;
using FluentValidation;
namespace Application.Validators.Session;

public class UpdateSessionStatusValidator : AbstractValidator<SessionDtos.SessionUpdateStatusDto>
{
    public UpdateSessionStatusValidator()
    {
        RuleFor(x => x.SessionStatus)
            .IsInEnum().WithMessage("Invalid SessionStatus");
    }
}