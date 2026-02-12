using Application.DTOs.ExternalMovieDto;
using Application.Services.ExternalMovie;
using Application.Services.Movie.MovieRepository;
using Domain.Entities;
using Domain.Interfaces;
using Moq;


public class MovieImportServiceTests
{
    private readonly Mock<IExternalMovieService> _externalService = new();
    private readonly Mock<IMovieRepository> _movieRepo = new();
    private readonly Mock<IActorRepository> _actorRepo = new();
    private readonly Mock<IGenreRepository> _genreRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    private MovieImportService CreateService()
        => new MovieImportService(
            _externalService.Object,
            _movieRepo.Object,
            _actorRepo.Object,
            _genreRepo.Object,
            _uow.Object
        );


    private ExternalActorDto CreateExternalActor()
        => new(
            123,
            "Tom Hardy",
            "https://image.tmdb.org/t/p/w500/tom.jpg",
            "Bane"
        );

    private ExternalMovieDto CreateExternalMovie()
        => new(
            TmdbId: 999,
            Title: "Test movie",
            Description: "Test description",
            Poster: "poster.jpg",
            Duration: 120,
            Language: "en",
            Rating: 7.5m,
            AgeRating: false,
            Genres: new List<string> { "Action" },
            Cast: new List<ExternalActorDto> { CreateExternalActor() },
            ReleaseDate: DateTime.UtcNow
        );


    [Fact]
    public async Task ImportFromTmdbAsync_ShouldAddMovie_WhenMovieDoesNotExist()
    {
        var movie = CreateExternalMovie();

        _externalService
            .Setup(s => s.GetPopularMovieIdsAsync(1))
            .ReturnsAsync(new List<int> { movie.TmdbId });

        _externalService
            .Setup(s => s.GetMovieAsync(movie.TmdbId))
            .ReturnsAsync(movie);

        _movieRepo
            .Setup(r => r.GetByTmdbIdAsync(movie.TmdbId))
            .ReturnsAsync((MovieEntity?)null);

        var service = CreateService();

        var result = await service.ImportFromTmdbAsync(1, CancellationToken.None);

        Assert.Equal(1, result.Added);
        _movieRepo.Verify(r => r.AddMovie(It.IsAny<MovieEntity>()), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }
}
