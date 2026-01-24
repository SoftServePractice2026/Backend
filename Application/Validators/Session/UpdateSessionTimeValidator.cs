using Application.DTOs;
using FluentValidation;
namespace Application.Validators.Session;


public class UpdateSessionTimeValidator : AbstractValidator<SessionDtos.SessionUpdateTimeDto>
{
    public UpdateSessionTimeValidator()
    {
        RuleFor(x => x)
            .Must(x => x.StartTime < x.EndTime)
            .WithMessage("StartTime must be less than EndTime");
    }
}