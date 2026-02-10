using Application.DTOs.ExternalMovieDto;
using Application.Services.Movie.MovieRepository;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.ExternalMovie
{
    public enum ImportStatus
    {
        Added,
        Updated,
        Ignored 
    }

    public class MovieImportService : IMovieImportService
    {
        private static readonly List<string> AvailableFormats = ["2D", "3D", "IMAX", "4DX", "ScreenX"];
        private static readonly Random Random = new();

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

        public async Task<ImportStatsDto> ImportFromTmdbAsync(int pagesCount, CancellationToken ct)
        {
            var added = 0;
            var updated = 0;
            var failed = 0;

            for (var i = 1; i <= pagesCount; i++)
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

                        var status = await SaveMovieInternalAsync(dto, ct);

                        await _unitOfWork.SaveChangesAsync();

                        if (status == ImportStatus.Added) added++;
                        else if (status == ImportStatus.Updated) updated++;
                    }
                    catch (Exception)
                    {
                        failed++;
                    }
                }
            }

            var total = added + updated + failed;
            return new ImportStatsDto(added, updated, failed, total);
        }

        private async Task<ImportStatus> SaveMovieInternalAsync(ExternalMovieDto dto, CancellationToken ct)
        {
            var existingMovies = await _movieRepository.GetByTmdbIdAsync(dto.TmdbId);
            var releaseDate = dto.ReleaseDate;

            if (existingMovies is not null)
            {
                existingMovies.Title = dto.Title;
                existingMovies.Description = dto.Description;
                existingMovies.Poster = dto.Poster;
                existingMovies.Rating = dto.Rating;

                await _unitOfWork.SaveChangesAsync(); 
                return ImportStatus.Updated;
            }

            var newMovie = MovieEntity.Create(
                dto.Poster,
                dto.Title,
                dto.Description,
                ParseAgeRating(dto.AgeRating),
                dto.Language,
                dto.Duration,
                releaseDate,
                releaseDate.AddMonths(2)
            );

            newMovie.TmdbId = dto.TmdbId;
            newMovie.Rating = dto.Rating;
            newMovie.Year = releaseDate.Year;
            newMovie.Formats = GenerateRandomFormats();

            newMovie.Genres = new List<GenreEntity>();
            if (dto.Genres != null)
            {
                foreach (var genreName in dto.Genres)
                {
                    var genre = await _genreRepository.GetGenreByNameAsync(genreName, ct);
                    if (genre == null)
                    {
                        genre = new GenreEntity
                        {
                            Id = Guid.NewGuid(),
                            Name = genreName
                        };

                    }
                    newMovie.Genres.Add(genre);
                }
            }

            newMovie.ActorsInMovies ??= new List<MovieActorEntity>();

            if (dto.Cast != null)
            {
                foreach (var castDto in dto.Cast)
                {
                    var actor = await _actorRepository.GetByTmdbIdAsync(castDto.TmdbId);

                    if (actor == null)
                    {
                        var names = castDto.FullName?.Split(' ') ?? ["Unknown"];
                        var firstName = names[0];
                        var lastName = names.Length > 1 ? string.Join(" ", names.Skip(1)) : "";

                        actor = new ActorEntity
                        {
                            Id = Guid.NewGuid(),
                            TmdbId = castDto.TmdbId,
                            FirstName = firstName,
                            LastName = lastName, 
                            Photo = castDto.PhotoUrl ?? "https://placehold.co/200x300?text=No+Photo"
                        };
                    }

                    var movieActor = new MovieActorEntity
                    {
                        MovieId = newMovie.Id,
                        Movie = newMovie,
                        ActorId = actor.Id,
                        Actor = actor,
                        CharacterName = castDto.Character
                    };

                    newMovie.ActorsInMovies.Add(movieActor);
                }
            }

            _movieRepository.AddMovie(newMovie);

            await _unitOfWork.SaveChangesAsync(ct);

            return ImportStatus.Added;

        }
        private static List<string> GenerateRandomFormats()
        {
            var count = Random.Next(1, 4);
            return AvailableFormats.OrderBy(_ => Random.Next()).Take(count).ToList();
        }

        private static int ParseAgeRating(bool isAdult)
        {
            return isAdult ? 18 : 12;
        }
    }
}
