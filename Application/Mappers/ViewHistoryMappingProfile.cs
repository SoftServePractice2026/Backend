using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
{
    public class ViewHistoryMappingProfile : Profile 
    {
        public ViewHistoryMappingProfile()
        {
            CreateMap<ViewHistoryCreateDto, ViewHistoryEntity>();

            CreateMap<ViewHistoryUpdateDto, ViewHistoryEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<ViewHistoryEntity, ViewHistoryDetailsDto>();
            CreateMap<ViewHistoryEntity, ViewHistoryListItemDto>();
        }
    }
}