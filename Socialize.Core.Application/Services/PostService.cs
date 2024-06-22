using Socialize.Core.Application.Dtos;
using Socialize.Core.Application.Repositories;
using Socialize.Core.Application.Services.Interfaces;
using Socialize.Core.Domain.Entities;
using System.Linq.Expressions;

namespace Socialize.Core.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<PostsPageDto> GetPosts(Guid? id, bool isNextPage, int pageSize, CancellationToken cancellationToken, bool readOnly, Expression<Func<Post, bool>> filter = null)
        {
            GetPostsDto getPostsDto = new GetPostsDto(id, isNextPage, pageSize);
            return await _postRepository.GetPosts(getPostsDto, cancellationToken, readOnly, filter);
        }
    }
}
