using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared;

namespace WebAPI.Filters
{
    public class ValidationFailureFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .SelectMany(kvp => kvp.Value!.Errors.Select(e =>
                    new Error(
                        "validation.error",
                        e.ErrorMessage,
                        ErrorType.VALIDATION,
                        kvp.Key)))
                    .ToList();

                context.Result = new BadRequestObjectResult(new Failure(errors));
            }
        }
    }
}
