using Microsoft.AspNetCore.Mvc;
using Shared;

namespace WebAPI.Mappers
{
    public static class FailureMapper
    {
        public static IActionResult ToHttp(Failure failure)
        {
            var errors = failure.Errors;

            if (errors.All(e => e.Type == ErrorType.VALIDATION))
            {
                var details = new ValidationProblemDetails(
                    errors.GroupBy(e => e.InvalidField ?? "")
                          .ToDictionary(
                              g => g.Key,
                              g => g.Select(x => x.Message).ToArray()
                          )
                )
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1", // 400
                    Title = "Validation errors occurred."
                };

                return new BadRequestObjectResult(details);
            }

            if (errors.All(e => e.Type == ErrorType.NOT_FOUND))
            {
                return new NotFoundObjectResult(new
                {
                    Errors = errors.Select(e => new { e.Code, e.Message, e.InvalidField })
                });
            }

            if (errors.All(e => e.Type == ErrorType.CONFLICT))
            {
                return new ConflictObjectResult(new
                {
                    Errors = errors.Select(e => new { e.Code, e.Message })
                });
            }

            if (errors.All(e => e.Type == ErrorType.UNATHORIZED))
            {
                return new UnauthorizedObjectResult(new
                {
                    Errors = errors.Select(e => new { e.Code, e.Message })
                });
            }

            // Інші помилки (Internal, Failure) → 500
            return new ObjectResult(new
            {
                Errors = errors.Select(e => new { e.Code, e.Message })
            })
            {
                StatusCode = 500
            };
        }
    }
}
