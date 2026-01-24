using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;

namespace WebAPI.Controllers
{
    [ApiController]
    [ServiceFilter(typeof(ValidationFailureFilter))]
    public class BaseController : ControllerBase
    {
    }
}
