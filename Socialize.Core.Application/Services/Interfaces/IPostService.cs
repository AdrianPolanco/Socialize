
using Socialize.Core.Application.Dtos;
using Socialize.Core.Domain.Entities;
using System.Linq.Expressions;

namespace Socialize.Core.Application.Services.Interfaces
{
    public interface IPostService
    {
        Task<PostsPageDto> GetPosts(Guid? id, bool isNextPage, int pageSize, CancellationToken cancellationToken, bool readOnly, Expression<Func<Post, bool>> filter = null);
    }
}
