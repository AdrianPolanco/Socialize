

using AutoMapper;
using Socialize.Core.Application.Dtos;
using Socialize.Core.Domain.Entities;
using Socialize.Core.Domain.Enums;

namespace Socialize.Core.Application.Mappers
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<Post, PostDto>()
             .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id))
             .ForCtorParam("Content", opt => opt.MapFrom(src => src.Content))
             .ForCtorParam("CreatedAt", opt => opt.MapFrom(src => src.CreatedAt))
             .ForCtorParam("Username", opt => opt.MapFrom(src => src.User.Username))
             .ForCtorParam("UsernamePhotoUrl", opt => opt.MapFrom(src => src.User.PhotoUrl))
             .ForCtorParam("Type", opt => opt.MapFrom(src => src.Attachment != null ? src.Attachment.Type : (AttachmentTypes?)null))
             .ForCtorParam("CommentsCount", opt => opt.MapFrom(src => src.Comments.Count))
             .ForCtorParam("AttachmentUrl", opt => opt.MapFrom(src => src.Attachment.Url));
        }
    }
}
