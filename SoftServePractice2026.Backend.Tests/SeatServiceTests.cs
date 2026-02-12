using Application.DTOs.Seat;
using Application.DTOs.Seat.SeatCRUD;
using Application.Services.Seat;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;
using Shared;
using Xunit;

namespace SoftServePractice2026.Backend.Tests.Services;

public class SeatServiceTests
{
    private readonly Mock<ISeatRepository> _seatRepo = new();
    private readonly Mock<IHallRepository> _hallRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IMapper> _mapper = new();

    private SeatService CreateService()
        => new SeatService(_hallRepo.Object, _uow.Object, _seatRepo.Object, _mapper.Object);

    [Fact]
    public async Task GetByIdAsync_ShouldReturnSeat_WhenExists()
    {
        var seatId = Guid.NewGuid();
        var hallId = Guid.NewGuid();
        var seat = new SeatEntity { Id = seatId };
        var dto = new SeatDto(seatId, hallId, 1, 1, default, default);

        _seatRepo.Setup(r => r.GetByIdAsync(seatId, default))
                 .ReturnsAsync(seat);

        _mapper.Setup(m => m.Map<SeatDto>(seat))
               .Returns(dto);

        var service = CreateService();

        var result = await service.GetByIdAsync(seatId);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(seatId);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldFail_WhenNotExists()
    {
        _seatRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), default))
                 .ReturnsAsync((SeatEntity?)null);

        var service = CreateService();

        var result = await service.GetByIdAsync(Guid.NewGuid());

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateSeat_WhenValid()
    {
        var dto = new CreateSeatDto(Guid.NewGuid(), 1, 2, default);
        var seat = new SeatEntity { Id = Guid.NewGuid() };
        var hall = new HallEntity { Id = Guid.NewGuid() };
        var resultDto = new SeatDto(seat.Id, hall.Id, 1, 2, default, default);

        _hallRepo.Setup(r => r.ExistsAsync(dto.HallId, default))
                 .ReturnsAsync(true);

        _seatRepo.Setup(r =>
                r.ExistsByPositionAsync(dto.HallId, dto.RowNumber, dto.SeatNumber, default))
            .ReturnsAsync(false);

        _mapper.Setup(m => m.Map<SeatEntity>(dto))
               .Returns(seat);

        _mapper.Setup(m => m.Map<SeatDto>(seat))
               .Returns(resultDto);

        var service = CreateService();

        var result = await service.CreateAsync(dto);

        result.IsSuccess.Should().BeTrue();
        _seatRepo.Verify(r => r.CreateSeat(seat), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldFail_WhenHallNotExists()
    {
        var dto = new CreateSeatDto(Guid.NewGuid(), 1, 1, default);

        _hallRepo.Setup(r => r.ExistsAsync(dto.HallId, default))
                 .ReturnsAsync(false);

        var service = CreateService();

        var result = await service.CreateAsync(dto);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteSeat_WhenNoTickets()
    {
        var seatId = Guid.NewGuid();
        var seat = new SeatEntity { Id = seatId };

        _seatRepo.Setup(r => r.GetByIdAsync(seatId, default))
                 .ReturnsAsync(seat);

        _seatRepo.Setup(r => r.HasTicketsAsync(seatId, default))
                 .ReturnsAsync(false);

        var service = CreateService();

        var result = await service.DeleteAsync(seatId);

        result.IsSuccess.Should().BeTrue();
        _seatRepo.Verify(r => r.DeleteSeat(seat), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldFail_WhenSeatHasTickets()
    {
        var seatId = Guid.NewGuid();
        var seat = new SeatEntity { Id = seatId };

        _seatRepo.Setup(r => r.GetByIdAsync(seatId, default))
                 .ReturnsAsync(seat);

        _seatRepo.Setup(r => r.HasTicketsAsync(seatId, default))
                 .ReturnsAsync(true);

        var service = CreateService();

        var result = await service.DeleteAsync(seatId);

        result.IsFailure.Should().BeTrue();
    }
}
