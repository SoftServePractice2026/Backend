using Application.DTOs.ExternalMovieDto;
using Application.Services.Movie.MovieRepository;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.ExternalMovie
{
    public enum ImportStatus
    {
        Added,
        Updated,
        Ignored 
    }

    public class MovieImportService
    {
        private readonly IExternalMovieService _externalMovieService;
        private readonly IMovieRepository _movieRepository;
        private readonly IActorRepository _actorRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IUnitOfWork _unitOfWork;
        public MovieImportService(IExternalMovieService externalMovieService, IMovieRepository movieRepository, IActorRepository actorRepository, IGenreRepository genreRepository, IUnitOfWork unitOfWork)
        {
            _externalMovieService = externalMovieService;
            _movieRepository = movieRepository;
            _actorRepository = actorRepository;
            _genreRepository = genreRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ImportStatsDto> ImportFromTmdbAsync(int pagesCount)
        {
            int added = 0;
            int updated = 0;
            int failed = 0;

            for (int i = 1; i <= pagesCount; i++)
            {
                var movieIds = await _externalMovieService.GetPopularMovieIdsAsync(i);

                foreach (var id in movieIds)
                {
                    try
                    {
                        var dto = await _externalMovieService.GetMovieAsync(id);

                        if (dto is null)
                        {
                            failed++;
                            continue;
                        }

                        var status = await SaveMovieInternalAsync(dto);

                        await _unitOfWork.SaveChangesAsync();

                        if (status == ImportStatus.Added) added++;
                        else if (status == ImportStatus.Updated) updated++;
                    }
                    catch (Exception ex)
                    {
                        failed++;
                    }
                }
            }

            int total = added + updated + failed;
            return new ImportStatsDto(added, updated, failed, total);
        }

        private async Task<ImportStatus> SaveMovieInternalAsync(ExternalMovieDto dto)
        {
            return ImportStatus.Added;
        }

    }
}
