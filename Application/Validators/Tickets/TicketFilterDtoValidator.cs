using Application.DTOs;
using Application.Validators.Common;
using Domain.Entities.Extensions;
using FluentValidation;

namespace Application.Validators.Tickets
{
    public class TicketFilterDtoValidator : PaginationBaseDtoValidator<TicketFilterDto>
    {
        public TicketFilterDtoValidator()
        {
            RuleFor(x => x.OrderBy)
                .Must(x => x == null || TicketOrederByMap.Map.ContainsKey(x.ToLower()))
                .When(x => x.OrderBy != null)
                .WithMessage("Invalid OrderBy column");
        }
    }
}