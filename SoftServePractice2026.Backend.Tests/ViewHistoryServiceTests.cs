using Application.DTOs;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Filters;
using Domain.Interfaces;
using FluentAssertions;
using Moq;
using Shared;
using Xunit;

namespace SoftServePractice2026.Backend.Tests.Services;

public class ViewHistoryServiceTests
{
    private readonly Mock<IViewHistoryRepository> _repo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    private ViewHistoryService CreateService()
        => new ViewHistoryService(_repo.Object, _mapper.Object, _uow.Object);


    [Fact]
    public async Task CreateViewHistoryAsync_ShouldReturnSuccess()
    {
        var dto = new ViewHistoryCreateDto(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow
        );

        var entity = new ViewHistoryEntity
        {
            Id = Guid.NewGuid(),
            UserId = dto.UserId,
            SessionId = dto.SessionId,
            ViewedAt = dto.ViewedAt
        };

        var detailsDto = new ViewHistoryDetailsDto(
            entity.Id,
            entity.UserId,
            entity.SessionId,
            entity.ViewedAt
        );

        _mapper.Setup(m => m.Map<ViewHistoryEntity>(dto)).Returns(entity);
        _mapper.Setup(m => m.Map<ViewHistoryDetailsDto>(entity)).Returns(detailsDto);

        var service = CreateService();

        var result = await service.CreateViewHistoryAsync(dto, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.UserId.Should().Be(dto.UserId);
    }


    [Fact]
    public async Task GetViewHistoryByIdAsync_ShouldFail_WhenNotFound()
    {
        _repo.Setup(r => r.GetViewHistoryByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync((ViewHistoryEntity?)null);

        var service = CreateService();

        var result = await service.GetViewHistoryByIdAsync(Guid.NewGuid(), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task GetViewHistoryByIdAsync_ShouldReturnDto_WhenExists()
    {
        var entity = new ViewHistoryEntity
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            SessionId = Guid.NewGuid(),
            ViewedAt = DateTime.UtcNow
        };

        var dto = new ViewHistoryDetailsDto(
            entity.Id,
            entity.UserId,
            entity.SessionId,
            entity.ViewedAt
        );

        _repo.Setup(r => r.GetViewHistoryByIdAsync(entity.Id, It.IsAny<CancellationToken>()))
             .ReturnsAsync(entity);

        _mapper.Setup(m => m.Map<ViewHistoryDetailsDto>(entity)).Returns(dto);

        var service = CreateService();

        var result = await service.GetViewHistoryByIdAsync(entity.Id, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(entity.Id);
    }


    [Fact]
    public async Task DeleteViewHistoryAsync_ShouldFail_WhenNotFound()
    {
        _repo.Setup(r => r.GetViewHistoryByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync((ViewHistoryEntity?)null);

        var service = CreateService();

        var result = await service.DeleteViewHistoryAsync(Guid.NewGuid(), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteViewHistoryAsync_ShouldReturnSuccess_WhenExists()
    {
        var entity = new ViewHistoryEntity { Id = Guid.NewGuid() };

        _repo.Setup(r => r.GetViewHistoryByIdAsync(entity.Id, It.IsAny<CancellationToken>()))
             .ReturnsAsync(entity);

        var service = CreateService();

        var result = await service.DeleteViewHistoryAsync(entity.Id, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }


    [Fact]
    public async Task UpdateViewHistoryAsync_ShouldFail_WhenNotFound()
    {
        _repo.Setup(r => r.GetViewHistoryByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync((ViewHistoryEntity?)null);

        var dto = new ViewHistoryUpdateDto(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow
        );

        var service = CreateService();

        var result = await service.UpdateViewHistoryAsync(Guid.NewGuid(), dto, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
    }


    [Fact]
    public async Task GetFilteredViewHistoryAsync_ShouldFail_WhenEmpty()
    {
        var filterDto = new ViewHistoryFilterDto(
            UserId: Guid.NewGuid(),
            SessionId: null,
            OrderBy: null
        );

        _repo.Setup(r => r.GetFilteredViewHistoryAsync(It.IsAny<ViewHistoryFilter>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(new List<ViewHistoryEntity>());

        var service = CreateService();

        var result = await service.GetFilteredViewHistoryAsync(filterDto, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task GetViewHistoryByUserIdAsync_ShouldReturnItems()
    {
        var userId = Guid.NewGuid();

        var entities = new List<ViewHistoryEntity>
        {
            new()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                SessionId = Guid.NewGuid(),
                ViewedAt = DateTime.UtcNow
            }
        };

        var listDtos = entities.Select(e =>
            new ViewHistoryListItemDto(e.Id, e.SessionId, e.ViewedAt)).ToList();

        _repo.Setup(r => r.GetFilteredViewHistoryAsync(It.IsAny<ViewHistoryFilter>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(entities);

        _mapper.Setup(m => m.Map<List<ViewHistoryListItemDto>>(entities))
               .Returns(listDtos);

        var service = CreateService();

        var result = await service.GetViewHistoryByUserIdAsync(userId, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
    }
}

