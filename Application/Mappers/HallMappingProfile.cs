using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
{
    public class HallMappingProfile : Profile 
    {
        public HallMappingProfile()
        {
            CreateMap<HallCreateDto, HallEntity>()
                .ConstructUsing(dto => new HallEntity() { 
                    Name = dto.Name,
                    IsActive = dto.IsActive,
                    HallSize = dto.HallSize 
                });

            CreateMap<HallUpdateDto, HallEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<HallEntity, HallDetailsDto>();
            CreateMap<HallEntity, HallListItemDto>();
        }
    }
}
