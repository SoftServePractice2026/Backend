using Application.DTOs.Seat;
using Application.DTOs.Seat.SeatCRUD;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using Shared;

namespace Application.Services.Seat;

public class SeatService : ISeatService
{
    private readonly ISeatRepository _seatRepository;
    private readonly IHallRepository _hallRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public SeatService(
        IHallRepository hallRepository,
        IUnitOfWork unitOfWork, 
        ISeatRepository seatRepository, 
        IMapper mapper)
    {
        _hallRepository = hallRepository;
        _unitOfWork = unitOfWork;
        _seatRepository = seatRepository;
        _mapper = mapper;
    }
    
    public async Task<Result<SeatDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var seat = await _seatRepository.GetByIdAsync(id, cancellationToken);

        if (seat is null)
        {
            return Result<SeatDto>.Fail(
                Error.NotFound("Seat.NotFound", "Seat not found"));
        }

        var dto = _mapper.Map<SeatDto>(seat);
        return Result<SeatDto>.Success(dto);
    }

    public async Task<Result<IEnumerable<SeatListDto>>> GetAllByHallIdAsync(Guid hallId, CancellationToken cancellationToken = default)
    {
        var hallExists = await _hallRepository.ExistsAsync(hallId, cancellationToken);

        if (!hallExists)
        {
            return Result<IEnumerable<SeatListDto>>.Fail(
                Error.NotFound("Hall.NotFound", $"Hall with id - {hallId} - not found"));
        }

        var seats = await _seatRepository.GetAllByHallIdAsync(hallId, cancellationToken);
        var dtos = _mapper.Map<IEnumerable<SeatListDto>>(seats);

        return Result<IEnumerable<SeatListDto>>.Success(dtos);
    }

    public async Task<Result<SeatDto>> CreateAsync(CreateSeatDto dto, CancellationToken cancellationToken = default)
    {
        var hallExists = await _hallRepository.ExistsAsync(dto.HallId, cancellationToken);

        if (!hallExists)
        {
            return Result<SeatDto>.Fail(
                Error.NotFound("Hall.NotFound", $"Hall with id - {dto.HallId} - not found"));
        }

        var seatExists = await _seatRepository.ExistsByPositionAsync(
            dto.HallId, 
            dto.RowNumber, 
            dto.SeatNumber, 
            cancellationToken);

        if (seatExists)
        {
            return Result<SeatDto>.Fail(
                Error.Conflict(
                    "Seat.AlreadyExists", 
                    $"Seat at row {dto.RowNumber}, number {dto.SeatNumber} already exists"));
        }

        var seat = _mapper.Map<SeatEntity>(dto);

        _seatRepository.CreateSeat(seat);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var resultDto = _mapper.Map<SeatDto>(seat);
        return Result<SeatDto>.Success(resultDto);
    }

    public async Task<Result<SeatDto>> UpdateAsync(UpdateSeatDto dto, CancellationToken cancellationToken = default)
    {
        var seat = await _seatRepository.GetByIdAsync(dto.Id, cancellationToken);

        if (seat is null)
        {
            return Result<SeatDto>.Fail(
                Error.NotFound("Seat.NotFound", "Seat not found"));
        }

        var existingSeatAtPosition = await _seatRepository.GetByPositionAsync(
            seat.HallId, 
            dto.RowNumber, 
            dto.SeatNumber, 
            cancellationToken);

        if (existingSeatAtPosition is not null && existingSeatAtPosition.Id != seat.Id)
        {
            return Result<SeatDto>.Fail(
                Error.Conflict(
                    "Seat.PositionTaken", 
                    $"Seat at row {dto.RowNumber}, number {dto.SeatNumber} already exists"));
        }

        seat.RowNumber = dto.RowNumber;
        seat.SeatNumber = dto.SeatNumber;
        seat.SeatType = dto.SeatType;
        seat.SeatStatus = dto.SeatStatus;

        _seatRepository.UpdateSeat(seat);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var resultDto = _mapper.Map<SeatDto>(seat);
        return Result<SeatDto>.Success(resultDto);
    }

    public async Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var seat = await _seatRepository.GetByIdAsync(id, cancellationToken);

        if (seat is null)
        {
            return Result<bool>.Fail(
                Error.NotFound("Seat.NotFound", "Seat not found"));
        }

        var hasTickets = await _seatRepository.HasTicketsAsync(id, cancellationToken);

        if (hasTickets)
        {
            return Result<bool>.Fail(
                Error.Conflict(
                    "Seat.HasTickets", 
                    "Cannot delete seat with existing tickets"));
        }

        _seatRepository.DeleteSeat(seat);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}