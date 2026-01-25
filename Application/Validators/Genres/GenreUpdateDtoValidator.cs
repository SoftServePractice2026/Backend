using Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validators.Genres
{
    public class GenreUpdateDtoValidator : AbstractValidator<GenreUpdateDto>
    {
        public GenreUpdateDtoValidator()
        {
            RuleFor(h => h.Name)
                .NotEmpty().WithMessage("Genre name is required").WithErrorCode("name.empty")
                .MinimumLength(1)
                .MaximumLength(30);

        }
    }
}
