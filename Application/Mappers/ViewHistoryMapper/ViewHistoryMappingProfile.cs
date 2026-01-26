using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Filters; 

namespace Application.Mappers
{
    public class ViewHistoryMappingProfile : Profile 
    {
        public ViewHistoryMappingProfile()
        {
            CreateMap<ViewHistoryCreateDto, ViewHistoryEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore());
            
            CreateMap<ViewHistoryUpdateDto, ViewHistoryEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore());
            
            CreateMap<ViewHistoryEntity, ViewHistoryDetailsDto>();
            CreateMap<ViewHistoryEntity, ViewHistoryListItemDto>();
            
            CreateMap<ViewHistoryFilter, ViewHistoryFilterDto>();
            CreateMap<ViewHistoryFilterDto, ViewHistoryFilter>();
        }
    }
}