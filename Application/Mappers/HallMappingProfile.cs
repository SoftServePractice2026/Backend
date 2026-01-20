using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
{
    public class HallMappingProfile : Profile 
    {
        public HallMappingProfile()
        {
            CreateMap<CreateHallDto, HallEntity>()
                .ConstructUsing(dto => new HallEntity() { 
                    Name = dto.Name,
                    IsActive = dto.IsActive,
                    HallSize = dto.HallSize 
                });

            CreateMap<HallEntity, HallDetailsDto>();
        }
    }
}
