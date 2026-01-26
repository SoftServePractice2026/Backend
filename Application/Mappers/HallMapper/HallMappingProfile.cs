using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Filters;

namespace Application.Mappers
{
    public class HallMappingProfile : Profile 
    {
        public HallMappingProfile()
        {
            CreateMap<HallCreateDto, HallEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.IsActive, opt => opt.Ignore());

            CreateMap<HallUpdateDto, HallEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<HallEntity, HallDetailsDto>();
            CreateMap<HallEntity, HallListItemDto>();

            CreateMap<HallFilter, HallFilterDto>();
            CreateMap<HallFilterDto, HallFilter>();
        }
    }
}
