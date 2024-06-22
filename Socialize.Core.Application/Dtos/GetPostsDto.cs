

namespace Socialize.Core.Application.Dtos
{
    public record GetPostsDto(
        Guid? Cursor,
        DateTime? CursorCreatedAt,
        bool IsNextPage,
        int PageSize = 5
        );
}
