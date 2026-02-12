using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.Genre;
using Application.Services.Genre;
using AutoMapper;
using Domain.Entities;
using Domain.Filters;
using Domain.Interfaces;
using FluentAssertions;
using Moq;
using Shared;
using Xunit;

namespace SoftServePractice2026.Backend.Tests.Services
{
    public class GenreServiceTests
    {
        private readonly Mock<IGenreRepository> _repository = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();

        private GenreService CreateService()
            => new GenreService(
                _repository.Object,
                _mapper.Object,
                _unitOfWork.Object
            );


        [Fact]
        public async Task CreateGenreAsync_ShouldCreate_WhenGenreDoesNotExist()
        {
            var dto = new GenreCreateDto("Action");
            var entity = new GenreEntity { Id = Guid.NewGuid(), Name = "Action" };
            var detailsDto = new GenreDetailsDto(entity.Id, entity.Name);

            _repository
                .Setup(r => r.GetGenreByNameAsync(dto.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync((GenreEntity?)null);

            _mapper.Setup(m => m.Map<GenreEntity>(dto)).Returns(entity);
            _mapper.Setup(m => m.Map<GenreDetailsDto>(entity)).Returns(detailsDto);

            var service = CreateService();

            var result = await service.CreateGenreAsync(dto, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Name.Should().Be("Action");

            _repository.Verify(r => r.CreateGenre(entity), Times.Once);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateGenreAsync_ShouldFail_WhenGenreAlreadyExists()
        {
            var dto = new GenreCreateDto("Action");

            _repository
                .Setup(r => r.GetGenreByNameAsync(dto.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GenreEntity { Name = "Action" });

            var service = CreateService();

            var result = await service.CreateGenreAsync(dto, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Failure.Errors.Should()
                .Contain(e => e.Code == "genre.exist");
        }



        [Fact]
        public async Task DeleteGenreAsync_ShouldDelete_WhenGenreExists()
        {
            var id = Guid.NewGuid();
            var entity = new GenreEntity { Id = id };

            _repository
                .Setup(r => r.GetGenreByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            var service = CreateService();

            var result = await service.DeleteGenreAsync(id, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeTrue();

            _repository.Verify(r => r.DeleteGenre(entity), Times.Once);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteGenreAsync_ShouldFail_WhenGenreNotFound()
        {
            var id = Guid.NewGuid();

            _repository
                .Setup(r => r.GetGenreByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((GenreEntity?)null);

            var service = CreateService();

            var result = await service.DeleteGenreAsync(id, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Failure.Errors.Should()
                .Contain(e => e.Code == "genre.not.found");
        }


        [Fact]
        public async Task GetGenreAllAsync_ShouldReturnList_WhenGenresExist()
        {
            var filterDto = new GenreFilterDto(null, null);
            var filter = new GenreFilter();
            var entities = new List<GenreEntity>
            {
                new() { Id = Guid.NewGuid(), Name = "Action" }
            };

            var listDtos = new List<GenreListItemDto>
            {
                new(entities[0].Id, "Action")
            };

            _mapper.Setup(m => m.Map<GenreFilter>(filterDto)).Returns(filter);
            _repository
                .Setup(r => r.GetAllGenresAsync(filter, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entities);

            _mapper.Setup(m => m.Map<List<GenreListItemDto>>(entities)).Returns(listDtos);

            var service = CreateService();

            var result = await service.GetGenreAllAsync(filterDto, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetGenreAllAsync_ShouldFail_WhenNoGenresFound()
        {
            var filterDto = new GenreFilterDto(
                Name: null,
                OrderBy: null);

            var filter = new GenreFilter();

            _mapper
                .Setup(m => m.Map<GenreFilter>(filterDto))
                .Returns(filter);

            _repository
                .Setup(r => r.GetAllGenresAsync(filter, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<GenreEntity>());

            var service = CreateService();

            var result = await service.GetGenreAllAsync(filterDto, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Failure.Errors.Should()
                .Contain(e => e.Code == "genres.not.found");
        }
  


        [Fact]
        public async Task UpdateGenreAsync_ShouldUpdate_WhenValid()
        {
            var id = Guid.NewGuid();
            var dto = new GenreUpdateDto("Drama");
            var entity = new GenreEntity { Id = id, Name = "Action" };

            _repository
                .Setup(r => r.GetGenreByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            _repository
                .Setup(r => r.GetGenreByNameAsync(dto.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync((GenreEntity?)null);

            _mapper.Setup(m => m.Map(dto, entity));

            _mapper.Setup(m => m.Map<GenreDetailsDto>(entity))
                .Returns(new GenreDetailsDto(id, "Drama"));

            var service = CreateService();

            var result = await service.UpdateGenreAsync(id, dto, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Name.Should().Be("Drama");
        }

        [Fact]
        public async Task UpdateGenreAsync_ShouldFail_WhenGenreNotFound()
        {
            var dto = new GenreUpdateDto("Drama");

            _repository
                .Setup(r => r.GetGenreByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GenreEntity?)null);

            var service = CreateService();

            var result = await service.UpdateGenreAsync(Guid.NewGuid(), dto, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Failure.Errors.Should()
                .Contain(e => e.Code == "genre.not.found");
        }
    }
}
