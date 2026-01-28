using Application.DTOs.Seat;
using Application.DTOs.Seat.SeatCRUD;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers.SeatMapper;

public class SeatMappingProfile : Profile
{
    public SeatMappingProfile()
    {
        // Entity -> DTO
        CreateMap<SeatEntity, SeatDto>();
        
        CreateMap<SeatEntity, SeatListDto>();

        // DTO -> Entity
        CreateMap<CreateSeatDto, SeatEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Hall, opt => opt.Ignore())
            .ForMember(dest => dest.Tickets, opt => opt.Ignore());

        CreateMap<UpdateSeatDto, SeatEntity>()
            .ForMember(dest => dest.HallId, opt => opt.Ignore())
            .ForMember(dest => dest.Hall, opt => opt.Ignore())
            .ForMember(dest => dest.Tickets, opt => opt.Ignore());
    }
}