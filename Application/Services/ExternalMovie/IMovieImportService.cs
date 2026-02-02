using Application.DTOs.ExternalMovieDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.ExternalMovie
{
    public interface IMovieImportService
    {
        Task<ImportStatsDto> ImportFromTmdbAsync(int pagesCount);
    }
}
