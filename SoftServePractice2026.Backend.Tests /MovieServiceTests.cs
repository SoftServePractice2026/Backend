using Application.Services.Movie.MovieService;
using Application.Services.Movie.MovieRepository;
using Application.Dtos.Movie;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;
using Shared;
using Xunit;

namespace SoftServePractice2026.Backend.Tests.Services;

public class MovieServiceTests
{
    private readonly Mock<IMovieRepository> _movieRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IMapper> _mapper = new();

    private MovieService CreateService()
        => new MovieService(_movieRepo.Object, _uow.Object, _mapper.Object);

    [Fact]
    public async Task CreateMovieAsync_ShouldReturnSuccess_WhenMovieNotExists()
    {
        var request = new CreateMovieDto(
            Title: "Interstellar",
            Description: "Sci-fi",
            Poster: "poster.jpg",
            AgeRating: 12,
            Language: "en",
            Duration: 169,
            RentalStartDate: DateTime.UtcNow,
            RentalEndDate: DateTime.UtcNow.AddDays(30),
            GenreIds: new List<Guid>()
        );

        _movieRepo
            .Setup(r => r.GetMovieByNameAsync(request.Title, It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<MovieEntity?>(null));

        var movieEntity = MovieEntity.Create(
            request.Poster,
            request.Title,
            request.Description,
            request.AgeRating,
            request.Language,
            request.Duration,
            request.RentalStartDate,
            request.RentalEndDate
        );

        _mapper
            .Setup(m => m.Map<MovieEntity>(request))
            .Returns(movieEntity);

        var detailsDto = new MovieDetailsDto(
            Guid.NewGuid(),
            request.Title,
            request.Description,
            request.Poster!,
            request.AgeRating,
            0,
            request.RentalStartDate,
            new List<string>(),
            new List<string>()
        );

        _mapper
            .Setup(m => m.Map<MovieDetailsDto>(movieEntity))
            .Returns(detailsDto);

        var service = CreateService();

        var result = await service.CreateMovieAsync(request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Title.Should().Be(request.Title);

        _movieRepo.Verify(r => r.AddMovie(movieEntity), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
