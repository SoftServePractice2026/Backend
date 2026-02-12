using Application.DTOs;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Filters;
using Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace SoftServePractice2026.Backend.Tests.Services
{
    public class HallServiceTests
    {
        private readonly Mock<IHallRepository> _repo = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IUnitOfWork> _uow = new();

        private HallService CreateService()
            => new HallService(_repo.Object, _mapper.Object, _uow.Object);

        [Fact]
        public async Task CreateHallAsync_ShouldCreateHall_WhenNotExists()
        {
            var dto = new HallCreateDto(
                "Main Hall",
                HallSizeEnum.Large
            );

            var entity = new HallEntity
            {
                Id = Guid.NewGuid(),
                Name = "Main Hall",
                HallSize = HallSizeEnum.Large,
                Seats = new List<SeatEntity>() 
            };

            var resultDto = new HallDetailsDto(
                entity.Id,
                entity.Name,
                entity.IsActive,
                entity.HallSize
            );

            _repo
                .Setup(r => r.GetHallByNameAsync(dto.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync((HallEntity)null);

            _mapper.Setup(m => m.Map<HallEntity>(dto)).Returns(entity);
            _mapper.Setup(m => m.Map<HallDetailsDto>(entity)).Returns(resultDto);

            var service = CreateService();

            var result = await service.CreateHallAsync(dto, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(resultDto);

            _repo.Verify(r => r.CreateHall(entity), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteHallAsync_ShouldFail_WhenNotFound()
        {
            var id = Guid.NewGuid();

            _repo
                .Setup(r => r.GetHallByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((HallEntity)null);

            var service = CreateService();

            var result = await service.DeleteHallAsync(id, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            _repo.Verify(r => r.DeleteHall(It.IsAny<HallEntity>()), Times.Never);
        }
    }
}
