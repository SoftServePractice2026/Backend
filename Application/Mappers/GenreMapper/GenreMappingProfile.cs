using Application.DTOs;
using Application.DTOs.Genre;
using AutoMapper;
using Domain.Entities;
using Domain.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Mappers.GenreMapper
{
    public class GenreMappingProfile : Profile
    {
        public GenreMappingProfile()
        {
            CreateMap<GenreCreateDto, GenreEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<GenreUpdateDto, GenreEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<GenreEntity, GenreDetailsDto>();
            CreateMap<GenreEntity, GenreListItemDto>();
        }
    }
}
