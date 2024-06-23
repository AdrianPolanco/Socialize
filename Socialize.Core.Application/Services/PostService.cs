using Socialize.Core.Application.Dtos;
using Socialize.Core.Application.Repositories;
using Socialize.Core.Application.Services.Interfaces;
using Socialize.Core.Domain.Entities;
using Socialize.Core.Domain.Repositories.Base;
using System.Linq.Expressions;

namespace Socialize.Core.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IRepository<Comment> _commentRepository;

        public PostService(IPostRepository postRepository, IRepository<Comment> commentRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        public async Task<PostsPageDto> GetPosts(Guid? id, bool isNextPage, int pageSize, CancellationToken cancellationToken, bool readOnly, Expression<Func<Post, bool>> filter = null)
        {
            GetPostsDto getPostsDto = new GetPostsDto(id, null, isNextPage, pageSize);
            return await _postRepository.GetPosts(getPostsDto, cancellationToken, readOnly, filter);
        }

        public async Task CommentAsync(Guid postId, Guid userId, string comment, CancellationToken cancellationToken)
        {
            Comment newComment = new()
            {
                Content = comment,
                PostId = postId,
                UserId = userId
            };

            await _commentRepository.CreateAsync(newComment, cancellationToken);
		}
    }
}
