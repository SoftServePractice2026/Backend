using Application.DTOs;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Validators.Session;

public class CreateSessionValidator : AbstractValidator<SessionCreateDto>
{
    public CreateSessionValidator(ISessionRepository sessionRepository)
    {
        RuleFor(x => x.MovieId)
            .NotEmpty().WithMessage("MovieId is required");


        RuleFor(x => x.HallId)
            .NotEmpty().WithMessage("HallId is required");


        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("StartTime is required");


        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("EndTime is required");

        
        RuleFor(x => x.StartTime)
            .Must(dt => dt.TimeOfDay >= new TimeSpan(8, 0, 0))
            .WithMessage("StartTime cannot be earlier than 08:00");

        
        RuleFor(x => x.EndTime)
            .Must(dt => dt.TimeOfDay <= new TimeSpan(20, 30, 0))
            .WithMessage("EndTime cannot be later than 20:30");
        

        RuleFor(x => x)
            .Must(x => x.StartTime < x.EndTime)
            .WithMessage("StartTime must be less than EndTime");

        
        RuleFor(x => x.SessionStatus)
            .IsInEnum().WithMessage("Invalid SessionStatus");

    }
}
