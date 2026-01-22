using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace Application.Mappers
{
    public class GenreMappingProfile : Profile
    {
        public GenreMappingProfile()
        {
            CreateMap<GenreCreateDto, GenreEntity>()
                .ConstructUsing(dto => new GenreEntity()
                {
                    Name = dto.Name
                });

            CreateMap<GenreUpdateDto, GenreEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<GenreEntity, GenreDetailsDto>();
            CreateMap<GenreEntity, GenreListItemDto>();
        }
    }
}
