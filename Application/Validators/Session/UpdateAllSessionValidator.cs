using Application.DTOs;
using FluentValidation;

namespace Application.Validators.Session;



public class UpdateAllSessionValidator : AbstractValidator<SessionDtos.SessionUpdateAllDto>
{
    public UpdateAllSessionValidator()
    {
        RuleFor(x => x)
            .Must(x => x.StartTime < x.EndTime)
            .WithMessage("StartTime must be less than EndTime");

        
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        
        RuleFor(x => x.SessionStatus)
            .IsInEnum().WithMessage("Invalid SessionStatus");
    }
}