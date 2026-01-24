using Application.DTOs;
using FluentValidation;
namespace Application.Validators.Sessions;

public class UpdateSessionPriceValidator : AbstractValidator<SessionDtos.SessionUpdatePriceDto>
{
    public UpdateSessionPriceValidator()
    {
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}