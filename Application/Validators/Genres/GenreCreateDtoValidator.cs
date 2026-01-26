using Application.DTOs.Genre;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validators.Genres
{
    public class GenreCreateDtoValidator : AbstractValidator<GenreCreateDto>
    {
        public GenreCreateDtoValidator()
        {
            RuleFor(h => h.Name)
                .NotEmpty().WithMessage("Genre name is required").WithErrorCode("name.empty")
                .MinimumLength(1)
                .MaximumLength(30);

        }
    }
}
