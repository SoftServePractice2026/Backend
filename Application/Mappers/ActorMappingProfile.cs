using Application.DTOs.Actor;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Mappers
{
    public class ActorMappingProfile : Profile
    {
        public ActorMappingProfile() 
        {
            CreateMap<ActorCreateDto, ActorEntity>()
                .ConstructUsing(dto => new ActorEntity()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName
                });


            CreateMap<ActorUpdateDto, ActorEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<ActorEntity, ActorDetailsDto>();
            CreateMap<ActorEntity, ActorListItemDto>();
        }
    }
}
