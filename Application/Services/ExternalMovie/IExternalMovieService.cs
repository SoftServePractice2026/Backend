using Application.DTOs.ExternalMovieDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.ExternalMovie
{
    public interface IExternalMovieService
    {
        Task<IEnumerable<int>> GetPopularMovieIdsAsync(int page);
        Task<ExternalMovieDto?> GetMovieAsync(int tmdbId);
    }
}
