using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Shared;

namespace Application.Services
{
    public interface ITicketService
    {
    Task<Result<TicketDetailsDto>> CreateTicketAsync(TicketCreateDto dto);
    Task<Result<TicketDetailsDto>> UpdateTicketAsync(Guid targetId, TicketUpdateDto dto);
    Task<Result<bool>> DeleteTicketAsync(Guid id);
    Task<Result<TicketDetailsDto>> GetTicketByIdAsync(Guid id);
    Task<Result<List<TicketListItemDto>>> GetTicketAllAsync();
    }

public class TicketService : ITicketService
    {
        private readonly ITicketRepository _repository;
        private readonly IMapper _mapper;

        public TicketService(ITicketRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<TicketDetailsDto>> CreateTicketAsync(TicketCreateDto dto)
        {
            var ticket = _mapper.Map<TicketEntity>(dto);

            try
            {
                await _repository.CreateTicketAsync(ticket);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Result<TicketDetailsDto>.Fail(
                    Failure.FromError(Error.Internal(message: ex.Message)));
            }

            var resultDto = _mapper.Map<TicketDetailsDto>(ticket);
            return Result<TicketDetailsDto>.Success(resultDto);
        }

        public async Task<Result<TicketDetailsDto>> GetTicketByIdAsync(Guid id)
        {
            var ticket = await _repository.GetTicketByIdAsync(id);

            if (ticket is null)
            {
                return Result<TicketDetailsDto>.Fail(
                    Failure.FromError(Error.NotFound("ticket.not.found", $"Ticket with id: {id} not found")));
            }

            var ticketDto = _mapper.Map<TicketDetailsDto>(ticket);
            return Result<TicketDetailsDto>.Success(ticketDto);
        }

        public async Task<Result<List<TicketListItemDto>>> GetTicketAllAsync()
        {
            var tickets = await _repository.GetTicketEntitiesAsync();
            var ticketsDto = _mapper.Map<List<TicketListItemDto>>(tickets);

            return Result<List<TicketListItemDto>>.Success(ticketsDto);
        }

        public async Task<Result<TicketDetailsDto>> UpdateTicketAsync(Guid targetId, TicketUpdateDto dto)
        {
            var ticket = await _repository.GetTicketByIdAsync(targetId);

            if (ticket is null)
            {
                return Result<TicketDetailsDto>.Fail(
                    Failure.FromError(Error.NotFound("ticket.not.found", $"Ticket with id: {targetId} not found")));
            }
            
            _mapper.Map(dto, ticket);

            await _repository.UpdateTicketAsync(ticket);
            await _repository.SaveChangesAsync();

            var updatedDto = _mapper.Map<TicketDetailsDto>(ticket);
            return Result<TicketDetailsDto>.Success(updatedDto);
        }

        public async Task<Result<bool>> DeleteTicketAsync(Guid id)
        {
            var ticket = await _repository.GetTicketByIdAsync(id);

            if (ticket is null)
            {
                return Result<bool>.Fail(
                    Failure.FromError(Error.NotFound("ticket.not.found", $"Ticket with id: {id} not found")));
            }

            await _repository.DeleteTicketAsync(ticket);
            await _repository.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}