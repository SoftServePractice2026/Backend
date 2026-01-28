using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validators.Actors
{
    public class ActorFilterDtoValidator : PaginationBaseDtoValidator<ActorFilterDto>
    {
        public ActorFilterDtoValidator() 
        {
            RuleFor(x => x.OrderBy)
                .Must(x => x == null || ActorOrederByMap.Map.ContainsKey(x.ToLower()))
                .When(x => x.OrderBy != null)
                .WithMessage("Invalid OrderBy column");
        }
    }
}
