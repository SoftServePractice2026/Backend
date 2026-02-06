using Application.Services.ExternalMovie;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/")]
    public class ImportController : ControllerBase
    {
        private readonly IMovieImportService _importService;

        public ImportController(IMovieImportService importService)
        {
            _importService = importService;
        }

        [HttpPost("seed")]
        public async Task<IActionResult> PopulateMovies(CancellationToken ct, [FromQuery] int pages = 1)
        {
            await _importService.ImportFromTmdbAsync(pages, ct);
            return Ok($"Import initiated for {pages} pages.");
        }
    }
}
