using Application.DTOs.Actor;
using Application.Validators.Common;
using Domain.Entities.Extensions;
using FluentValidation;

namespace Application.Validators.Actors
{
    public class ActorFilterDtoValidator : PaginationBaseDtoValidator<ActorFilterDto>
    {
        public ActorFilterDtoValidator() 
        {
            RuleFor(x => x.OrderBy)
                .Must(x => x == null || ActorOrderByMap.Map.ContainsKey(x.ToLower()))
                .When(x => x.OrderBy != null)
                .WithMessage("Invalid OrderBy column");
        }
    }
}
