using Application.DTOs;
using FluentValidation;

namespace Application.Validators.Session;



public class UpdateAllSessionValidator : AbstractValidator<SessionUpdateDto>
{
    public UpdateAllSessionValidator()
    {
        RuleFor(x => x)
            .Must(x => x.StartTime < x.EndTime)
            .When(x => x.StartTime.HasValue && x.EndTime.HasValue)
            .WithMessage("StartTime must be less than EndTime");
        
        RuleFor(x => x.SessionStatus)
            .IsInEnum().WithMessage("Invalid SessionStatus");
    }
}