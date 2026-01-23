using Application.Dtos.Movie;
using FluentValidation;

namespace Application.Validators.Movies;

public class DeleteMovieValidator : AbstractValidator<DeleteMovieDto>
{
    public DeleteMovieValidator()
    {
        RuleFor(m => m.Id)
            .NotEmpty()
            .WithMessage("Movie id is required");
        
    }
}