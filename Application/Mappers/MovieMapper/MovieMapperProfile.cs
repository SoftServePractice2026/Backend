using Application.Dtos.Movie;
using AutoMapper;
using Domain.Entities;
using Domain.Filters;

namespace Application.Mappers.MovieMapper;

public class MovieMapperProfile : Profile
{
    public MovieMapperProfile()
    {
        CreateMap<MovieEntity, MovieDetailsDto>()
            .ConstructUsing(src => new MovieDetailsDto(
                src.Id,
                src.Title,
                src.Description,
                src.Poster ?? string.Empty,
                src.AgeRating,
                src.Rating ?? 0m,
                src.RentalStartDate,
                src.Genres.Select(g => g.Name).ToList()
            ))
            .ForAllMembers(opt => opt.Ignore());
            
        
        CreateMap<UpdateMovieDto, MovieEntity>()
            .ForMember(x => x.Id, opt => opt.Ignore());


        CreateMap<MovieEntity, MovieListItemDto>()
            .ConstructUsing(src => new MovieListItemDto(
                src.Id,
                src.Title,
                src.Genres.Select(g => g.Id).FirstOrDefault()
                ));
        
        CreateMap<MovieFilter, MovieFilterDto>();
        CreateMap<MovieFilterDto, MovieFilter>();
        
    }
}