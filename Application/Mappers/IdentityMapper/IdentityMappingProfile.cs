using Application.Dtos;
using Application.Dtos.Identity;
using AutoMapper;
using Infrastructure.Identity.Data;

namespace Application.Mappers;

public class IdentityMappingProfile : Profile
{
    public IdentityMappingProfile()
    {
        //CreateMap<ApplicationUser, IdentityDetailsDto>()
        //    .ConstructUsing(src => new IdentityDetailsDto(
        //        src.Id,
        //        src.UserName!,
        //        src.Email!))
        //    .ForAllMembers(opt => opt.Ignore());

        CreateMap<RegisterRequest, ApplicationUser>()
            .ConstructUsing(src => new ApplicationUser()
            {
                Email = src.Email,
                UserName = src.Email,
                FirstName = src.FirstName,
                LastName = src.LastName,
                BirthDate = src.BirthDate,
            })
            .ForAllMembers(opt => opt.Ignore());
    }
}