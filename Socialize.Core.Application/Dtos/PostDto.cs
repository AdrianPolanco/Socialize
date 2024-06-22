
using Socialize.Core.Domain.Enums;

namespace Socialize.Core.Application.Dtos
{
    public record PostDto(
        Guid Id,
        string Content,
        DateTime CreatedAt,
        string Username,
        string UsernamePhotoUrl,
        AttachmentTypes? Type,
        int CommentsCount,
        string? AttachmentUrl
        );
}
