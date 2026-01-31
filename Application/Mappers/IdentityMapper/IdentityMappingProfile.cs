using Application.Dtos;
using AutoMapper;
using Infrastructure.Identity.Data;

namespace Application.Mappers;

public class IdentityMappingProfile : Profile
{
    public IdentityMappingProfile()
    {
        CreateMap<ApplicationUser, IdentityDetailsDto>()
            .ConstructUsing(src => new IdentityDetailsDto(
                src.Id,
                src.UserName,
                src.Email,
                new List<string>()))
            .ForAllMembers(opt => opt.Ignore());
    }
    
}