

using Socialize.Core.Application.Dtos;
using Socialize.Core.Domain.Entities;
using System.Linq.Expressions;

namespace Socialize.Core.Application.Repositories
{
    public interface IPostRepository
    {
        Task<PostsPageDto> GetPosts(GetPostsDto getPostsDto, CancellationToken cancellationToken, bool readOnly, Expression<Func<Post, bool>> filter = null);
    }
}
