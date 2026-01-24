namespace Application.Mappers.SessionMapper;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;



public class SessionMappingProfile : Profile
{
    public SessionMappingProfile()
    {
        CreateMap<SessionDtos.SessionCreateDto, SessionEntity>();
        CreateMap<SessionDtos.SessionUpdateAllDto, SessionEntity>()
            .ForMember(x => x.Id, opt =>
                opt.Ignore())
            
            .ForMember(x => x.MovieId, opt =>
                opt.Ignore())
            
            .ForMember(x => x.HallId, opt => 
                opt.Ignore())
            
            .ForMember(x => x.Movie, opt =>
                opt.Ignore())
            
            .ForMember(x => x.Hall, opt =>
                opt.Ignore())
            
            .ForMember(x => x.Tickets, opt =>
                opt.Ignore())
            
            .ForMember(x => x.ViewHistories, opt =>
                opt.Ignore());

        CreateMap<SessionDtos.SessionUpdateTimeDto, SessionEntity>()
            .ForMember(x => x.Id, opt =>
                opt.Ignore());

        
        CreateMap<SessionDtos.SessionUpdateStatusDto, SessionEntity>()
            .ForMember(x => x.Id, opt =>
                opt.Ignore());

            CreateMap<SessionEntity, SessionDtos.SessionCardDto>()
                .ForCtorParam("MovieTitle", opt =>
                    opt.MapFrom(s => s.Movie != null ? s.Movie.Title : string.Empty))
                .ForCtorParam("HallName", opt =>
                    opt.MapFrom(s => s.Hall != null ? s.Hall.Name : string.Empty))
                .ForCtorParam("Price", opt =>
                    opt.MapFrom(_ => 0m))
                .ForCtorParam("Seats", opt =>
                    opt.MapFrom(_ => new List<SessionDtos.SeatInSessionDto>()));
        
        
        }
}
