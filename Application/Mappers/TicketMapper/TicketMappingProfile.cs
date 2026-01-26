using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Filters;

namespace Application.Mappers
{
    public class TicketMappingProfile : Profile 
    {
        public TicketMappingProfile()
        {
            CreateMap<TicketCreateDto, TicketEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<TicketUpdateDto, TicketEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<TicketEntity, TicketDetailsDto>();
            CreateMap<TicketEntity, TicketListItemDto>();
            
            CreateMap<TicketFilter, TicketFilterDto>();
            CreateMap<TicketFilterDto, TicketFilter>();
        }
    }
}
