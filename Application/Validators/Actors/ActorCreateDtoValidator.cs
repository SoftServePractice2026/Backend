using Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Application.Validators.Actors
{
    public class ActorCreateDtoValidator : AbstractValidator<ActorCreateDto>
    {
        public ActorCreateDtoValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Actor name is required").WithErrorCode("name.empty")
                .MinimumLength(1)
                .MaximumLength(50);

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Actor surname is required").WithErrorCode("surname.empty")
                .MinimumLength(1)
                .MaximumLength(50);
        }
    }
}
