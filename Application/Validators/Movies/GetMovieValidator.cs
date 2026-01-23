using Application.Dtos.Movie;
using FluentValidation;

namespace Application.Validators.Movies;

public class GetMovieValidator : AbstractValidator<GetMovieByIdDto>
{
    public GetMovieValidator()
    {
        RuleFor(m => m.Id)
            .NotEmpty()
            .WithMessage("Movie id is required");
    }
}