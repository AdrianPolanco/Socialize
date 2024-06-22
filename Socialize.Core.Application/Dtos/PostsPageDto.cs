

using Socialize.Core.Domain.Entities;

namespace Socialize.Core.Application.Dtos
{
    public record PostsPageDto(
        ICollection<PostDto> Posts,
        Guid? NextId,
        Guid? PreviousId,
        bool IsFirstPage
    );
}
