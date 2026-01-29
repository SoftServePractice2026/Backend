using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validators.Genres
{
    public class GenresFilterDtoValidator : PaginationBaseDtoValidator<GenreListItemDto>
    {
        public GenreFilterDtoValidator()
        {
            RuleFor(x => x.OrderBy)
                .Must(x => x == null || GenreOrederByMap.Map.ContainsKey(x.ToLower()))
                .When(x => x.OrderBy != null)
                .WithMessage("Invalid orderBy column");
        }
    }
}
