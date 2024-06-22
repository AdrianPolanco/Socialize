

using Socialize.Core.Application.Dtos;

namespace Socialize.Core.Application.UseCases.ReadPosts
{
    public interface IReadPostsUseCase
    {
        Task<PostsPageDto> GetPosts(Guid? id, bool isNextPage, Guid? currentPageId, CancellationToken cancellationToken);
    }
}
