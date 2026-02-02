using Application.Services.ExternalMovie;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportController : ControllerBase
    {
        private readonly IMovieImportService _importService;

        public ImportController(IMovieImportService importService)
        {
            _importService = importService;
        }

        [HttpPost("populate")]
        public async Task<IActionResult> PopulateMovies([FromQuery] int pages = 1)
        {
            await _importService.ImportFromTmdbAsync(pages);
            return Ok($"Import initiated for {pages} pages.");
        }
    }
}
