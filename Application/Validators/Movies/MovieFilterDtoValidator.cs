using Application.Dtos.Movie;
using Application.Validators.Common;
using Domain.Entities.Extensions;
using FluentValidation;

namespace Application.Validators.Movies;

public class MovieFilterDtoValidator : PaginationBaseDtoValidator<MovieFilterDto>
{
    public MovieFilterDtoValidator()
    {
        RuleFor(x => x.OrderBy)
            .Must(x => x == null || MovieOrderByMap.Map.ContainsKey(x.ToLower()))
            .When(x => x.OrderBy != null)
            .WithMessage("Invalid OrderBy column");
    }
}