using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ServiceFilter(typeof(ValidationFailureFilter))]
    public class BaseController : ControllerBase
    {
    }
}
