using Application.DTOs;
using FluentValidation;

namespace Application.Validators.ViewHistory
{
    public class ViewHistoryCreateDtoValidator : AbstractValidator<ViewHistoryCreateDto>
    {
        public ViewHistoryCreateDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required").WithErrorCode("userId.empty");

            RuleFor(x => x.SessionId)
                .NotEmpty().WithMessage("Session ID is required").WithErrorCode("sessionId.empty");

            RuleFor(x => x.ViewedAt)
                .NotEmpty().WithMessage("Viewed date is required")
                .WithErrorCode("viewedAt.empty")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Viewed date cannot be in the future")
                .WithErrorCode("viewedAt.invalid");
        }
    }
}   