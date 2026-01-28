using Application.DTOs.Actor;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validators.Actors
{
    public class ActorUpdateDtoValidator : AbstractValidator<ActorUpdateDto>
    {
        public ActorUpdateDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Actor name is required").WithErrorCode("name.empty")
                .MinimumLength(1)
                .MaximumLength(50);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Actor surname is required").WithErrorCode("surname.empty")
                .MinimumLength(1)
                .MaximumLength(50);
        }
    }
}
