using Application.DTOs;
using Application.Services.Session;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Filters;
using Domain.Interfaces;
using FluentAssertions;
using Moq;
using Shared;
using Xunit;

namespace SoftServePractice2026.Backend.Tests.Services;

public class SessionServiceTests
{
    private readonly Mock<ISessionRepository> _sessionRepo = new();
    private readonly Mock<ITicketRepository> _ticketRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IMapper> _mapper = new();

    private SessionService CreateService()
        => new SessionService(
            _sessionRepo.Object,
            _ticketRepo.Object,
            _uow.Object,
            _mapper.Object);

    [Fact]
    public async Task CreateSessionAsync_ShouldFail_WhenOverlapExists()
    {
        var dto = new SessionCreateDto(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(2),
            SessionStatusEnum.Active
        );

        _sessionRepo
            .Setup(r => r.HasOverlapAsync(
                dto.HallId,
                dto.StartTime,
                dto.EndTime,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var service = CreateService();

        var result = await service.CreateSessionAsync(dto, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task CreateSessionAsync_ShouldCreate_WhenNoOverlap()
    {
        var dto = new SessionCreateDto(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(2),
            SessionStatusEnum.Active
        );

        var session = new SessionEntity { Id = Guid.NewGuid() };

        _sessionRepo
            .Setup(r => r.HasOverlapAsync(
                dto.HallId,
                dto.StartTime,
                dto.EndTime,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _mapper
            .Setup(m => m.Map<SessionEntity>(dto))
            .Returns(session);

        var service = CreateService();

        var result = await service.CreateSessionAsync(dto, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(session.Id);
    }

    [Fact]
    public async Task GetSessionByIdAsync_ShouldReturnDto_WhenFound()
    {
        var session = new SessionEntity { Id = Guid.NewGuid() };

        var dto = new SessionListItemDto(
            session.Id,
            "Movie",
            Guid.NewGuid(),
            "Hall",
            Guid.NewGuid(),
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(2),
            SessionStatusEnum.Active
        );

        _sessionRepo
            .Setup(r => r.GetSessionByIdAsync(session.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);

        _mapper
            .Setup(m => m.Map<SessionListItemDto>(session))
            .Returns(dto);

        var service = CreateService();

        var result = await service.GetSessionByIdAsync(session.Id, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(session.Id);
    }

    [Fact]
    public async Task UpdateSessionAsync_ShouldFail_WhenNotFound()
    {
        _sessionRepo
            .Setup(r => r.GetSessionByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SessionEntity?)null);

        var service = CreateService();

        var result = await service.UpdateSessionAsync(
            Guid.NewGuid(),
            new SessionUpdateDto(null, null, SessionStatusEnum.Cancelled),
            CancellationToken.None);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task GetFilteredSessionsAsync_ShouldReturnSessions()
    {
        var filterDto = new SessionFilterDto(
            null, null,
            null, null,
            null, null,
            null, null
        );

        var filter = new SessionFilter();

        var sessions = new List<SessionEntity>
        {
            new() { Id = Guid.NewGuid() }
        };

        var list = new List<SessionListItemDto>
        {
            new(
                sessions[0].Id,
                "Movie",
                Guid.NewGuid(),
                "Hall",
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                SessionStatusEnum.Active
            )
        };

        _mapper.Setup(m => m.Map<SessionFilter>(filterDto)).Returns(filter);
        _sessionRepo.Setup(r => r.GetFilteredSessionsAsync(filter, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(sessions);
        _sessionRepo.Setup(r => r.CountFilteredAsync(filter, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(1);
        _mapper.Setup(m => m.Map<List<SessionListItemDto>>(sessions))
               .Returns(list);

        var service = CreateService();

        var result = await service.GetFilteredSessionsAsync(filterDto, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Sessions.Should().HaveCount(1);
        result.Value.TotalCount.Should().Be(1);
    }
}

