using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
{
    public class TicketMappingProfile : Profile 
    {
        public TicketMappingProfile()
        {
            CreateMap<TicketCreateDto, TicketEntity>();

            CreateMap<TicketUpdateDto, TicketEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<TicketEntity, TicketDetailsDto>();
            CreateMap<TicketEntity, TicketListItemDto>();
        }
    }
}
