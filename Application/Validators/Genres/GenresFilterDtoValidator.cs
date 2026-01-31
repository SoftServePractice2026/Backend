using Application.DTOs.Genre;
using Application.Validators.Common;
using Domain.Entities.Extensions;
using FluentValidation;

namespace Application.Validators.Genres
{
    public class GenresFilterDtoValidator : PaginationBaseDtoValidator<GenreFilterDto>
    {
        public GenresFilterDtoValidator()
        {
            RuleFor(x => x.OrderBy)
                .Must(x => x == null || GenreOrederByMap.Map.ContainsKey(x.ToLower()))
                .When(x => x.OrderBy != null)
                .WithMessage("Invalid orderBy column");
        }
    }
}
