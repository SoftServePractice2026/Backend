using Application.DTOs.ExternalMovieDto;
using Application.Services.ExternalMovie;
using Application.Services.Movie.MovieRepository;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace SoftServePractice2026.Backend.Tests.Services
{
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

        [Fact]
        public async Task ImportFromTmdbAsync_WhenMovieDoesNotExist_ShouldAddMovie()
        {
            // Arrange
            var dto = new ExternalMovieDto(
                1,
                "Test movie",
                "Desc",
                "poster.jpg",
                120,
                "en",
                8.0m,
                false,
                new List<string>(),
                new List<ExternalActorDto>(),
                DateTime.UtcNow
            );

            _externalService
                .Setup(s => s.GetPopularMovieIdsAsync(1))
                .ReturnsAsync(new List<int> { 1 });

            _externalService
                .Setup(s => s.GetMovieAsync(1))
                .ReturnsAsync(dto);

            _movieRepo
                .Setup(r => r.GetByTmdbIdAsync(1))
                .ReturnsAsync((MovieEntity)null);

            var service = CreateService();

            // Act
            var result = await service.ImportFromTmdbAsync(1, CancellationToken.None);

            // Assert
            result.Added.Should().Be(1);
            result.Updated.Should().Be(0);
            result.Failed.Should().Be(0);

            _movieRepo.Verify(r => r.AddMovie(It.IsAny<MovieEntity>()), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }
    }
}