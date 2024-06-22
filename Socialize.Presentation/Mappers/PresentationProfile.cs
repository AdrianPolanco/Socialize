using AutoMapper;
using Socialize.Core.Application.Dtos;
using Socialize.Core.Domain.Entities;
using Socialize.Core.Domain.Enums;
using Socialize.Infrastructure.Identity.Models;
using Socialize.Presentation.Models.Posts;
using Socialize.Presentation.Models.Users;
using System.Globalization;

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

            CreateMap<LoginUserViewModel, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
                .ReverseMap();

            // Mapeo de PostDto a PostViewModel
            CreateMap<PostDto, PostViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.CreatedAtFormatted, opt => opt.MapFrom(src => $"{src.CreatedAt:MMMM dd, yyyy} at {src.CreatedAt:HH:mm}"))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.UsernamePhoto, opt => opt.MapFrom(src => src.UsernamePhotoUrl))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.CommentsCount, opt => opt.MapFrom(src => src.CommentsCount))
                .ForMember(dest => dest.AttachmentUrl, opt => opt.MapFrom(src => src.AttachmentUrl));

            // Mapeo de PostViewModel a PostDto
            CreateMap<PostViewModel, PostDto>()
                .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id))
                .ForCtorParam("Content", opt => opt.MapFrom(src => src.Content))
                .ForCtorParam("CreatedAt", opt => opt.MapFrom(src => src.CreatedAt))
                .ForCtorParam("Username", opt => opt.MapFrom(src => src.Username))
                .ForCtorParam("UsernamePhotoUrl", opt => opt.MapFrom(src => src.UsernamePhoto))
                .ForCtorParam("Type", opt => opt.MapFrom(src => src.Type != null? src.Type: null))
                .ForCtorParam("CommentsCount", opt => opt.MapFrom(src => src.CommentsCount))
                .ForCtorParam("AttachmentUrl", opt => opt.MapFrom(src => src.AttachmentUrl));

            // Mapeo de PostPageViewModel a PostsPageDto
            CreateMap<PostPageViewModel, PostsPageDto>()
                .ForMember(dest => dest.Posts, opt => opt.MapFrom(src => src.Posts))
                .ForMember(dest => dest.NextId, opt => opt.MapFrom(src => src.NextId))
                .ForMember(dest => dest.PreviousId, opt => opt.MapFrom(src => src.PreviousId))
                .ForMember(dest => dest.IsFirstPage, opt => opt.MapFrom(src => src.IsFirstPage))
                .ReverseMap();
        }
    }
}
