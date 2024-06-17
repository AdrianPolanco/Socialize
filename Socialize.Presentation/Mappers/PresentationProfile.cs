using AutoMapper;
using Socialize.Core.Domain.Entities;
using Socialize.Presentation.Models.Users;

namespace Socialize.Presentation.Mappers
{
    public class PresentationProfile : Profile
    {
        public PresentationProfile()
        {
            CreateMap<RegisterUserViewModel, User>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => src.Lastname))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
                .ReverseMap();
        }
    }
}
