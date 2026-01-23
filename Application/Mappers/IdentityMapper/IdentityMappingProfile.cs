using Application.Dtos;
using AutoMapper;
using Infrastructure.Identity.Data;

namespace Application.Mappers;

public class IdentityMappingProfile : Profile
{
    public IdentityMappingProfile()
    {
        CreateMap<ApplicationUser, IdentityDetailsDto>()
            .ForMember(x => x.Name, opt
                => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))

            .ForMember(x => x.Roles, opt
                => opt.Ignore());
    }
    
}