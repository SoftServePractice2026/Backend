using Application.DTOs;
using Application.Services.Ticket;
using AutoMapper;
using Domain.Entities;
using Domain.Filters;
using Domain.Interfaces;
using FluentAssertions;
using Domain.Entities.Enums;
using Moq;
using Shared;
using Xunit;

namespace SoftServePractice2026.Backend.Tests.Services;

public class TicketServiceTests
{
    private readonly Mock<ITicketRepository> _repo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    private TicketService CreateService()
        => new TicketService(_repo.Object, _mapper.Object, _uow.Object);


    [Fact]
    public async Task CreateTicketAsync_ShouldFail_WhenSeatAlreadyTaken()
    {
        var dto = new TicketCreateDto(
            UserId: Guid.NewGuid(),
            SeatId: Guid.NewGuid(),
            SessionId: Guid.NewGuid(),
            Price: 150,
            TicketStatus: TicketStatusEnum.Reserved
        );

        _repo.Setup(r =>
                r.GetTicketBySeatAndSessionAsync(dto.SeatId, dto.SessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TicketEntity());

        var service = CreateService();

        var result = await service.CreateTicketAsync(dto, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task CreateTicketAsync_ShouldCreate_WhenSeatIsFree()
    {
        var dto = new TicketCreateDto(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            150,
            TicketStatusEnum.Reserved
        );

        var entity = new TicketEntity();

        var detailsDto = new TicketDetailsDto(
            Guid.NewGuid(),
            dto.UserId,
            dto.SeatId,
            dto.SessionId,
            Guid.NewGuid(),
            dto.Price,
            dto.TicketStatus
        );

        _repo.Setup(r =>
                r.GetTicketBySeatAndSessionAsync(dto.SeatId, dto.SessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TicketEntity?)null);

        _mapper.Setup(m => m.Map<TicketEntity>(dto))
               .Returns(entity);

        _mapper.Setup(m => m.Map<TicketDetailsDto>(entity))
               .Returns(detailsDto);

        var service = CreateService();

        var result = await service.CreateTicketAsync(dto, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(detailsDto);
    }


    [Fact]
    public async Task GetTicketByIdAsync_ShouldFail_WhenNotFound()
    {
        _repo.Setup(r => r.GetTicketByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync((TicketEntity?)null);

        var service = CreateService();

        var result = await service.GetTicketByIdAsync(Guid.NewGuid(), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task GetTicketByIdAsync_ShouldReturnTicket_WhenExists()
    {
        var entity = new TicketEntity();

        var dto = new TicketDetailsDto(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            200,
            TicketStatusEnum.Paid
        );

        _repo.Setup(r => r.GetTicketByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(entity);

        _mapper.Setup(m => m.Map<TicketDetailsDto>(entity))
               .Returns(dto);

        var service = CreateService();

        var result = await service.GetTicketByIdAsync(Guid.NewGuid(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(dto);
    }


    [Fact]
    public async Task UpdateTicketAsync_ShouldFail_WhenNotFound()
    {
        _repo.Setup(r => r.GetTicketByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync((TicketEntity?)null);

        var service = CreateService();

        var result = await service.UpdateTicketAsync(
            Guid.NewGuid(),
            new TicketUpdateDto(200, TicketStatusEnum.Paid),
            CancellationToken.None);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateTicketAsync_ShouldUpdate_WhenExists()
    {
        var entity = new TicketEntity();

        var dto = new TicketDetailsDto(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            200,
            TicketStatusEnum.Paid
        );

        _repo.Setup(r => r.GetTicketByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(entity);

        _mapper.Setup(m => m.Map(It.IsAny<TicketUpdateDto>(), entity));
        _mapper.Setup(m => m.Map<TicketDetailsDto>(entity)).Returns(dto);

        var service = CreateService();

        var result = await service.UpdateTicketAsync(
            Guid.NewGuid(),
            new TicketUpdateDto(200, TicketStatusEnum.Paid),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(dto);
    }


    [Fact]
    public async Task DeleteTicketAsync_ShouldFail_WhenNotFound()
    {
        _repo.Setup(r => r.GetTicketByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync((TicketEntity?)null);

        var service = CreateService();

        var result = await service.DeleteTicketAsync(Guid.NewGuid(), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteTicketAsync_ShouldDelete_WhenExists()
    {
        var entity = new TicketEntity();

        _repo.Setup(r => r.GetTicketByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(entity);

        var service = CreateService();

        var result = await service.DeleteTicketAsync(Guid.NewGuid(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();
    }


    [Fact]
    public async Task GetFilteredTicketsAsync_ShouldFail_WhenEmpty()
    {
        var filterDto = new TicketFilterDto(null, null, null);
        var filter = new TicketFilter();

        _mapper.Setup(m => m.Map<TicketFilter>(filterDto)).Returns(filter);
        _repo.Setup(r => r.GetFilteredTicketsAsync(filter, It.IsAny<CancellationToken>()))
             .ReturnsAsync(new List<TicketEntity>());

        var service = CreateService();

        var result = await service.GetFilteredTicketsAsync(filterDto, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task GetFilteredTicketsAsync_ShouldReturnList_WhenExists()
    {
        var filterDto = new TicketFilterDto(null, null, null);
        var filter = new TicketFilter();

        var entities = new List<TicketEntity> { new() };
        var listDto = new List<TicketListItemDto>
        {
            new(Guid.NewGuid(), Guid.NewGuid(), 150, TicketStatusEnum.Paid)
        };

        _mapper.Setup(m => m.Map<TicketFilter>(filterDto)).Returns(filter);
        _repo.Setup(r => r.GetFilteredTicketsAsync(filter, It.IsAny<CancellationToken>()))
             .ReturnsAsync(entities);
        _repo.Setup(r => r.CountFilteredAsync(filter, It.IsAny<CancellationToken>()))
             .ReturnsAsync(1);
        _mapper.Setup(m => m.Map<List<TicketListItemDto>>(entities))
               .Returns(listDto);

        var service = CreateService();

        var result = await service.GetFilteredTicketsAsync(filterDto, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.TotalCount.Should().Be(1);
        result.Value.Tickets.Should().HaveCount(1);
    }
}
