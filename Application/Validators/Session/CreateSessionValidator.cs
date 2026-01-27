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


        RuleFor(x => x)
            .Must(x => x.StartTime < x.EndTime)
            .WithMessage("StartTime must be less than EndTime");

        
        RuleFor(x => x.SessionStatus)
            .IsInEnum().WithMessage("Invalid SessionStatus");

    }
}
