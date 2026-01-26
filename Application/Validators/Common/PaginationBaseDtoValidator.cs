using Application.DTOs.Common;
using FluentValidation;

namespace Application.Validators.Common
{
    public abstract class PaginationBaseDtoValidator<T> : AbstractValidator<T> where T : PaginationBaseDto
    {
        public PaginationBaseDtoValidator()
        {
            RuleFor(p => p.PageNumber)
                .GreaterThan(0);

            RuleFor(p => p.PageSize)
                .GreaterThan(0)
                .LessThan(30);
        }
    }
}
