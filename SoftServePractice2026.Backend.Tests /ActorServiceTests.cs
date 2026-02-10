using Application.DTOs.Actor;
using Application.Services.Actor;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;
using Domain.Filters;

namespace SoftServePractice2026.Backend.Tests.Services
{
    public class ActorServiceTests
    {
        private readonly Mock<IActorRepository> _repo = new();
        private readonly Mock<IUnitOfWork> _uow = new();
        private readonly Mock<IMapper> _mapper = new();

        private ActorService CreateService()
        {
            return new ActorService(
                _repo.Object,
                _mapper.Object,
                _uow.Object
            );
        }



        [Fact]
        public async Task CreateActorAsync_ShouldReturnSuccess()
        {
            var dto = new ActorCreateDto("Tom", "Hardy");

            var entity = new ActorEntity
            {
                Id = Guid.NewGuid(),
                FirstName = "Tom",
                LastName = "Hardy"
            };

            var detailsDto = new ActorDetailsDto(entity.Id, entity.FirstName, entity.LastName);

            _mapper.Setup(m => m.Map<ActorEntity>(dto)).Returns(entity);
            _mapper.Setup(m => m.Map<ActorDetailsDto>(entity)).Returns(detailsDto);

            var service = CreateService();

            var result = await service.CreateActorAsync(dto, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.FirstName.Should().Be("Tom");

            _repo.Verify(r => r.CreateActor(entity), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }


        [Fact]
        public async Task GetActorByIdAsync_ShouldReturnActor_WhenExists()
        {
            var id = Guid.NewGuid();

            var entity = new ActorEntity
            {
                Id = id,
                FirstName = "Tom",
                LastName = "Hardy"
            };

            var dto = new ActorDetailsDto(id, "Tom", "Hardy");

            _repo.Setup(r => r.GetActorByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            _mapper.Setup(m => m.Map<ActorDetailsDto>(entity)).Returns(dto);

            var service = CreateService();

            var result = await service.GetActorByIdAsync(id, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.LastName.Should().Be("Hardy");
        }

        [Fact]
        public async Task GetActorByIdAsync_ShouldFail_WhenNotFound()
        {
            _repo.Setup(r => r.GetActorByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ActorEntity?)null);

            var service = CreateService();

            var result = await service.GetActorByIdAsync(Guid.NewGuid(), CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Failure.Errors.Should().Contain(e => e.Code == "actor.not.found");
        }


        [Fact]
        public async Task UpdateActorAsync_ShouldUpdate_WhenExists()
        {
            var id = Guid.NewGuid();
            var dto = new ActorUpdateDto("Leonardo", "DiCaprio");

            var entity = new ActorEntity
            {
                Id = id,
                FirstName = "Old",
                LastName = "Name"
            };

            var detailsDto = new ActorDetailsDto(id, "Leonardo", "DiCaprio");

            _repo.Setup(r => r.GetActorByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            _mapper.Setup(m => m.Map(dto, entity));
            _mapper.Setup(m => m.Map<ActorDetailsDto>(entity)).Returns(detailsDto);

            var service = CreateService();

            var result = await service.UpdateActorAsync(id, dto, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.FirstName.Should().Be("Leonardo");

            _repo.Verify(r => r.UpdateActor(entity), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateActorAsync_ShouldFail_WhenNotFound()
        {
            _repo.Setup(r => r.GetActorByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ActorEntity?)null);

            var service = CreateService();

            var result = await service.UpdateActorAsync(
                Guid.NewGuid(),
                new ActorUpdateDto("A", "B"),
                CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Failure.Errors.Should().Contain(e => e.Code == "actor.not.found");
        }


        [Fact]
        public async Task DeleteActorAsync_ShouldDelete_WhenExists()
        {
            var entity = new ActorEntity { Id = Guid.NewGuid() };

            _repo.Setup(r => r.GetActorByIdAsync(entity.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            var service = CreateService();

            var result = await service.DeleteActorAsync(entity.Id, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            _repo.Verify(r => r.DeleteActor(entity), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteActorAsync_ShouldFail_WhenNotFound()
        {
            _repo.Setup(r => r.GetActorByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ActorEntity?)null);

            var service = CreateService();

            var result = await service.DeleteActorAsync(Guid.NewGuid(), CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Failure.Errors.Should().Contain(e => e.Code == "actor.not.found");
        }


        [Fact]
        public async Task GetFilteredActorsAsync_ShouldReturnList()
        {
            var filterDto = new ActorFilterDto(null, null, null);
            var filter = new ActorFilter();

            var entities = new List<ActorEntity>
            {
                new ActorEntity { Id = Guid.NewGuid(), FirstName = "Tom", LastName = "Hardy" }
            };

            var listDtos = new List<ActorListItemDto>
            {
                new ActorListItemDto(entities[0].Id, "Tom", "Hardy", "")
            };

            _mapper.Setup(m => m.Map<ActorFilter>(filterDto)).Returns(filter);
            _repo.Setup(r => r.GetFilteredActorsAsync(filter, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entities);
            _repo.Setup(r => r.CountFilteredAsync(filter, It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);
            _mapper.Setup(m => m.Map<List<ActorListItemDto>>(entities)).Returns(listDtos);

            var service = CreateService();

            var result = await service.GetFilteredActorsAsync(filterDto, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.TotalCount.Should().Be(1);
        }
    }
}
