using Application.Dtos.Movie;
using FluentValidation;

namespace Application.Validators.Movies;

public class CreateMovieValidator : AbstractValidator<CreateMovieDto>
{
    public CreateMovieValidator()
    {
        RuleFor(m => m.Title)
            .NotEmpty()
            .WithMessage("Movie title is required")
            .WithErrorCode("title.empty")
            .MinimumLength(3)
            .MaximumLength(100);
        
        RuleFor(m => m.Description)
            .NotEmpty().WithMessage("Movie description is required")
            .WithErrorCode("description.empty")
            .MinimumLength(10)
            .MaximumLength(500);

        RuleFor(m => m.Duration)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Movie duration must be greater than 0 minutes")
            .WithErrorCode("duration.invalid");


        RuleFor(m => m.AgeRating)
            .NotEmpty()
            .WithMessage("Movie age rating is required");
        
        RuleFor(m => m.Language)
            .NotEmpty()
            .WithMessage("Movie language is required");
        
        
        RuleFor(m => m.RentalStartDate)
            .NotEmpty()
            .WithMessage("Movie rental start date is required");
        
        RuleFor(m => m.RentalEndDate)
            .NotEmpty()
            .GreaterThan(m => m.RentalStartDate)
            .WithMessage("Movie rental end date is required");

        RuleFor(m => m.GenreIds)
            .NotEmpty()
            .WithMessage("Movie must have at least one genre")
            .Must(ids => ids != null && ids.Any()).WithMessage("Movie must have at least one genre");

    }
}