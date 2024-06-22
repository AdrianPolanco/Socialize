

namespace Socialize.Core.Application.Dtos
{
    public record GetPostsDto(
        Guid? Cursor,
        bool IsNextPage,
        int PageSize = 5
        );
}
