namespace Application.Mappers.SessionMapper;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;

public class SessionMappingProfile : Profile
{
    public SessionMappingProfile()
    {
        CreateMap<SessionCreateDto, SessionEntity>()
            .ForMember(x => x.Id, opt => opt.Ignore());
        
        CreateMap<SessionUpdateDto, SessionEntity>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.MovieId, opt => opt.Ignore())
            .ForMember(d => d.HallId, opt => opt.Ignore())
            .ForMember(d => d.Movie, opt => opt.Ignore())
            .ForMember(d => d.Hall, opt => opt.Ignore())
            .ForMember(d => d.Tickets, opt => opt.Ignore())
            .ForMember(d => d.ViewHistories, opt => opt.Ignore())

            .ForMember(d => d.StartTime, opt =>
            {
                opt.PreCondition(s => s.StartTime.HasValue);
                opt.MapFrom(s => s.StartTime!.Value);
            })
            .ForMember(d => d.EndTime, opt =>
            {
                opt.PreCondition(s => s.EndTime.HasValue);
                opt.MapFrom(s => s.EndTime!.Value);
            })
            .ForMember(d => d.SessionStatus, opt =>
            {
                opt.PreCondition(s => s.SessionStatus.HasValue);
                opt.MapFrom(s => s.SessionStatus!.Value);
            });

        
        
        CreateMap<SessionEntity, SessionListItemDto>()
            .ConstructUsing(s => new SessionListItemDto(
                s.Id,
                s.Movie == null ? string.Empty : s.Movie.Title,
                s.MovieId,
                s.Hall == null ? string.Empty : s.Hall.Name,
                s.HallId,
                s.StartTime,
                s.EndTime,
                s.SessionStatus
                ));
    }
}
