using Application.DTOs;
using Application.Validators.Common;
using Domain.Entities.Extensions;
using FluentValidation;

namespace Application.Validators.Halls
{
    public class HallFilterDtoValidator : PaginationBaseDtoValidator<HallFilterDto>
    {
        public HallFilterDtoValidator()
        {
            RuleFor(x => x.OrderBy)
                .Must(x => x == null || HallOrederByMap.Map.ContainsKey(x.ToLower()))
                .When(x => x.OrderBy != null)
                .WithMessage("Invalid OrderBy column");
        }
    }
}
