using Application.Services.Movie.MovieService;
using Application.Services.Movie.MovieRepository;
using Application.Dtos.Movie;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace SoftServePractice2026.Backend.Tests.Services;

public class MovieServiceTests
{
    private readonly Mock<IMovieRepository> _movieRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IMapper> _mapper = new();

    private MovieService CreateService()
        => new MovieService(
            _movieRepository.Object,
            _unitOfWork.Object,
            _mapper.Object
        );

    private static CreateMovieDto CreateCreateDto()
        => new CreateMovieDto(
            Title: "Test movie",
            Description: "Description",
            Poster: "poster.jpg",
            Language: "en",
            AgeRating: 16,
            Duration: 120,
            RentalStartDate: DateTime.UtcNow,
            RentalEndDate: DateTime.UtcNow.AddDays(7),
            GenreIds: new List<Guid>()
        );

    private static MovieEntity CreateMovieEntity()
        => MovieEntity.Create(
            poster: "poster.jpg",
            title: "Test movie",
            description: "Description",
            ageRating: 16,
            language: "en",
            duration: 120,
            start: DateTime.UtcNow,
            end: DateTime.UtcNow.AddDays(7)
        );

    private static MovieDetailsDto CreateMovieDetailsDto()
        => new MovieDetailsDto(
            Id: Guid.NewGuid(),
            Title: "Test movie",
            Description: "Description",
            Poster: "poster.jpg",
            AgeRating: 16,
            Rating: 7.5m,
            RentalStartDate: DateTime.UtcNow,
            Actors: new List<string>(),
            Genres: new List<string>()
        );


    [Fact]
    public async Task CreateMovieAsync_ShouldCreate_WhenMovieDoesNotExist()
    {
        var dto = CreateCreateDto();
        var entity = CreateMovieEntity();
        var detailsDto = CreateMovieDetailsDto();

        _movieRepository
            .Setup(r => r.GetMovieByNameAsync(dto.Title, It.IsAny<CancellationToken>()))
            .ReturnsAsync((MovieEntity?)null);

        _mapper.Setup(m => m.Map<MovieEntity>(dto)).Returns(entity);
        _mapper.Setup(m => m.Map<MovieDetailsDto>(entity)).Returns(detailsDto);

        var service = CreateService();

        var result = await service.CreateMovieAsync(dto, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Title.Should().Be(dto.Title);

        _movieRepository.Verify(r => r.AddMovie(entity), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateMovieAsync_ShouldFail_WhenMovieAlreadyExists()
    {
        var dto = CreateCreateDto();

        _movieRepository
            .Setup(r => r.GetMovieByNameAsync(dto.Title, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateMovieEntity());

        var service = CreateService();

        var result = await service.CreateMovieAsync(dto, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Should()
            .Contain(e => e.Code == "Movie.Exist");

    }


    [Fact]
    public async Task GetMovieByIdAsync_ShouldReturnMovie_WhenExists()
    {
        var id = Guid.NewGuid();
        var entity = CreateMovieEntity();
        var dto = CreateMovieDetailsDto();

        _movieRepository
            .Setup(r => r.GetMovieByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        _mapper.Setup(m => m.Map<MovieDetailsDto>(entity)).Returns(dto);

        var service = CreateService();

        var result = await service.GetMovieByIdAsync(
            new GetMovieByIdDto(id),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Title.Should().Be(dto.Title);
    }

    [Fact]
    public async Task GetMovieByIdAsync_ShouldFail_WhenNotFound()
    {
        var id = Guid.NewGuid();

        _movieRepository
            .Setup(r => r.GetMovieByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((MovieEntity?)null);

        var service = CreateService();

        var result = await service.GetMovieByIdAsync(
            new GetMovieByIdDto(id),
            CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Should()
            .Contain(e => e.Code == "MovieNotFound");

    }


    [Fact]
    public async Task DeleteMovieAsync_ShouldDelete_WhenExists()
    {
        var id = Guid.NewGuid();
        var entity = CreateMovieEntity();

        _movieRepository
            .Setup(r => r.GetMovieByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        var service = CreateService();

        var result = await service.DeleteMovieAsync(
            new DeleteMovieDto(id),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _movieRepository.Verify(r => r.DeleteMovie(entity), Times.Once);
    }

    [Fact]
    public async Task DeleteMovieAsync_ShouldFail_WhenNotFound()
    {
        var id = Guid.NewGuid();

        _movieRepository
            .Setup(r => r.GetMovieByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((MovieEntity?)null);

        var service = CreateService();

        var result = await service.DeleteMovieAsync(
            new DeleteMovieDto(id),
            CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Should()
            .Contain(e => e.Code == "MovieNotFound");

    }
}
