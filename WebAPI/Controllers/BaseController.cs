using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;

namespace WebAPI.Controllers
{
    [ApiController]
    [ServiceFilter(typeof(ValidationFailureFilter))]
    public class BaseController : ControllerBase
    {
        protected Guid GetUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User id not found in token");

            return Guid.Parse(userId);
        }
    }
}
