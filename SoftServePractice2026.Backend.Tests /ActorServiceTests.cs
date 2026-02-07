using Application.DTOs.Actor;
using Application.Services.Actor;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace SoftServePractice2026.Backend.Tests.Services
{
    public class ActorServiceTests
    {
        private readonly Mock<IActorRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _uowMock;

        private readonly ActorService _service;

        public ActorServiceTests()
        {
            _repoMock = new Mock<IActorRepository>();
            _mapperMock = new Mock<IMapper>();
            _uowMock = new Mock<IUnitOfWork>();

            _service = new ActorService(
                _repoMock.Object,
                _mapperMock.Object,
                _uowMock.Object
            );
        }

        [Fact]
        public async Task CreateActorAsync_ShouldCreateActor_AndReturnSuccess()
        {
            var createDto = new ActorCreateDto("Tom", "Hardy");

            var actorEntity = new ActorEntity
            {
                Id = Guid.NewGuid(),
                FirstName = "Tom",
                LastName = "Hardy"
            };

            var detailsDto = new ActorDetailsDto(
                actorEntity.Id,
                actorEntity.FirstName,
                actorEntity.LastName
            );

            _mapperMock
                .Setup(m => m.Map<ActorEntity>(createDto))
                .Returns(actorEntity);

            _mapperMock
                .Setup(m => m.Map<ActorDetailsDto>(actorEntity))
                .Returns(detailsDto);


            var result = await _service.CreateActorAsync(createDto, CancellationToken.None);


            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(detailsDto);

            _repoMock.Verify(r => r.CreateActor(actorEntity), Times.Once);
            _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}