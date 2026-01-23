using Application.Dtos.Movie;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers.MovieMapper;

public class MovieMapperProfile : Profile
{
    public MovieMapperProfile()
    {
        CreateMap<MovieEntity, MovieDetailsDto>()

            .ForMember(x => x.Genres, opt
                => opt.MapFrom(src => src.Genres.Select(g => g.Name)));
        
        CreateMap<UpdateMovieDto, MovieEntity>()
            .ForMember(x => x.Id, opt => opt.Ignore());

    }
}