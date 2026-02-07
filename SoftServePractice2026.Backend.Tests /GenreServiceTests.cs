using Application.DTOs.Genre;
using Application.Services.Genre;
using AutoMapper;
using Domain.Entities;
using Domain.Filters;
using Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace SoftServePractice2026.Backend.Tests.Services
{
    public class GenreServiceTests
    {
        private readonly Mock<IGenreRepository> _repo = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IUnitOfWork> _uow = new();

        private GenreService CreateService()
            => new GenreService(
                _repo.Object,
                _mapper.Object,
                _uow.Object
            );

        [Fact]
        public async Task CreateGenreAsync_ShouldCreateGenre_WhenNotExists()
        {
            var dto = new GenreCreateDto("Drama");

            var entity = new GenreEntity
            {
                Id = Guid.NewGuid(),
                Name = "Drama"
            };

            var resultDto = new GenreDetailsDto(entity.Id, entity.Name);

            _repo
                .Setup(r => r.GetGenreByNameAsync(dto.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync((GenreEntity)null);

            _mapper.Setup(m => m.Map<GenreEntity>(dto)).Returns(entity);
            _mapper.Setup(m => m.Map<GenreDetailsDto>(entity)).Returns(resultDto);

            var service = CreateService();

           
            var result = await service.CreateGenreAsync(dto, CancellationToken.None);

          
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(resultDto);

            _repo.Verify(r => r.CreateGenre(entity), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateGenreAsync_ShouldFail_WhenGenreAlreadyExists()
        {
            
            var dto = new GenreCreateDto("Drama");

            _repo
                .Setup(r => r.GetGenreByNameAsync(dto.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GenreEntity());

            var service = CreateService();

            var result = await service.CreateGenreAsync(dto, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            _repo.Verify(r => r.CreateGenre(It.IsAny<GenreEntity>()), Times.Never);
        }

        [Fact]
        public async Task GetGenreByIdAsync_ShouldReturnGenre_WhenExists()
        {
            var id = Guid.NewGuid();

            var entity = new GenreEntity
            {
                Id = id,
                Name = "Action"
            };

            var dto = new GenreDetailsDto(id, "Action");

            _repo
                .Setup(r => r.GetGenreByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            _mapper.Setup(m => m.Map<GenreDetailsDto>(entity)).Returns(dto);

            var service = CreateService();

            var result = await service.GetGenreByIdAsync(id, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(dto);
        }

        [Fact]
        public async Task DeleteGenreAsync_ShouldDelete_WhenExists()
        {
            var entity = new GenreEntity
            {
                Id = Guid.NewGuid(),
                Name = "Comedy"
            };

            _repo
                .Setup(r => r.GetGenreByIdAsync(entity.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            var service = CreateService();

            var result = await service.DeleteGenreAsync(entity.Id, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            _repo.Verify(r => r.DeleteGenre(entity), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
