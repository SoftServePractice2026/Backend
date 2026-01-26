using Application.DTOs;
using Application.DTOs.Actor;
using AutoMapper;
using Domain.Entities;
using Domain.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Mappers.ActorMapper
{
    public class ActorMappingProfile : Profile
    {
        public ActorMappingProfile()
        {
            CreateMap<ActorCreateDto, ActorEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore());


            CreateMap<HallUpdateDto, HallEntity>()
                 .ForMember(x => x.Id, opt => opt.Ignore());


            CreateMap<ActorEntity, ActorDetailsDto>();

            CreateMap<ActorEntity, ActorListItemDto>();


            CreateMap<ActorFilterDto, ActorFilter>();
            CreateMap<ActorFilter, ActorFilterDto>();
        }
    }
}
