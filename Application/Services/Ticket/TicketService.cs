using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Shared;
using Domain.Filters;
namespace Application.Services.Ticket;

public class TicketService : ITicketService
    {
        private readonly ITicketRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TicketService(ITicketRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<TicketDetailsDto>> CreateTicketAsync(TicketCreateDto dto, CancellationToken cancellationToken)
        {
            var isSeatTaken = await _repository.GetTicketBySeatAndSessionAsync(dto.SeatId, dto.SessionId, cancellationToken);

            if (isSeatTaken is not null)
            {
                var conflictError = Error.Conflict("ticket.seat.taken", $"Seat {dto.SeatId} for session {dto.SessionId} is already booked.");
                return Result<TicketDetailsDto>.Fail(conflictError);
            }

            var ticket = _mapper.Map<TicketEntity>(dto);
            
            _repository.CreateTicket(ticket);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            var resultDto = _mapper.Map<TicketDetailsDto>(ticket);
                
            return Result<TicketDetailsDto>.Success(resultDto);
        }
        
        public async Task<Result<TicketDetailsDto>> GetTicketByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var ticket = await _repository.GetTicketByIdAsync(id, cancellationToken);

            if (ticket is null)
            {
                var error = Error.NotFound("ticket.not.found", $"Ticket with id: {id} not found");
                return Result<TicketDetailsDto>.Fail(error);
            }
            
            return Result<TicketDetailsDto>.Success(_mapper.Map<TicketDetailsDto>(ticket));
        }
        
        public async Task<Result<TicketDetailsDto>> UpdateTicketAsync(Guid targetId, TicketUpdateDto dto, CancellationToken cancellationToken)
        {
            var ticket = await _repository.GetTicketByIdAsync(targetId, cancellationToken);

            if (ticket is null)
            {
                var error = Error.NotFound("ticket.not.found", $"Ticket with id: {targetId} not found");
                return Result<TicketDetailsDto>.Fail(error);
            }
            
            _mapper.Map(dto, ticket);
            _repository.UpdateTicket(ticket);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return Result<TicketDetailsDto>.Success(_mapper.Map<TicketDetailsDto>(ticket));
        }

        public async Task<Result<bool>> DeleteTicketAsync(Guid id, CancellationToken cancellationToken)
        {
            var ticket = await _repository.GetTicketByIdAsync(id, cancellationToken);

            if (ticket is null)
            {
                var error = Error.NotFound("ticket.not.found", $"Ticket with id: {id} not found");
                return Result<bool>.Fail(error);
            }

            _repository.DeleteTicket(ticket);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }

        public async Task<Result<(List<TicketListItemDto> Tickets, int TotalCount)>> GetFilteredTicketsAsync(TicketFilterDto ticketFilterDto, CancellationToken cancellationToken)
        {
            var filter = _mapper.Map<TicketFilter>(ticketFilterDto);
            var tickets = await _repository.GetFilteredTicketsAsync(filter, cancellationToken);

            if (!tickets.Any())
            {
                var error = Error.NotFound("tickets.not.found", "No tickets found for given filter");
                return Result<(List<TicketListItemDto> Tickets, int TotalCount)>.Fail(error);
            }

            var totalCount = await _repository.CountFilteredAsync(filter, cancellationToken);
            var ticketsDto = _mapper.Map<List<TicketListItemDto>>(tickets);

            return Result<(List<TicketListItemDto> Tickets, int TotalCount)>.Success((ticketsDto, totalCount));
        }
    }
