using Application.DTOs;
using Application.Validators.Common;
using Domain.Entities.Extensions;
using FluentValidation;

namespace Application.Validators.ViewHistory
{
    public class ViewHistoryFilterDtoValidator : PaginationBaseDtoValidator<ViewHistoryFilterDto>
    {
        public ViewHistoryFilterDtoValidator()
        {
            RuleFor(x => x.OrderBy)
                .Must(x => x == null || ViewHistoryOrederByMap.Map.ContainsKey(x.ToLower()))
                .When(x => x.OrderBy != null)
                .WithMessage("Invalid OrderBy column");
        }
    }
}