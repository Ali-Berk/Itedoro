using AutoMapper;
using Itedoro.Business.Services.LoginService;
using Itedoro.Data.Entities.Users;
public class LoginMappingProfile : Profile
{
    public LoginMappingProfile()
    {
        CreateMap<User, UserProfileDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name));

        CreateMap<User, LoginResponseDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.AccessToken, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
            .ForMember(dest => dest.ExpiresAt, opt => opt.Ignore())
            .ForMember(dest => dest.Message, opt => opt.Ignore());
    }
}