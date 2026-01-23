using Application.Dtos.Movie;
using FluentValidation;

namespace Application.Validators.Movies;

public class UpdateMovieValidator : AbstractValidator<UpdateMovieDto>
{
    public UpdateMovieValidator()
    {
        RuleFor(m => m.Id)
            .NotEmpty()
            .WithMessage("Movie id is required")
            .WithErrorCode("id.empty");
        
        RuleFor(m => m.Title)
            .NotEmpty()
            .WithMessage("Movie title is required")
            .WithErrorCode("title.empty")
            .MinimumLength(3)
            .MaximumLength(100);

        RuleFor(m => m.Description)
            .NotEmpty()
            .WithMessage("Movie description is required")
            .WithErrorCode("description.empty")
            .MinimumLength(10)
            .MaximumLength(500);
        
        RuleFor(m => m.Duration)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Movie duration must be greater than 0 minutes")
            .WithErrorCode("duration.invalid");

        RuleFor(m => m.Poster)
            .MaximumLength(1000)
            .WithMessage("Poster url must be less than 1000 characters");
        
        RuleFor(m => m.AgeRating)
            .NotEmpty()
            .WithMessage("Movie age rating is required");
        
        RuleFor(m => m.Language)
            .NotEmpty()
            .WithMessage("Movie language is required");
    }
}