using Socialize.Core.Application.Dtos;
using Socialize.Core.Application.Services.Interfaces;

namespace Socialize.Core.Application.UseCases.ReadPosts
{
    public class ReadPostsUseCase : IReadPostsUseCase
    {
        private readonly IPostService _postService;

        public ReadPostsUseCase(IPostService postService)
        {
            _postService = postService;
        }
        public async Task<PostsPageDto> GetPosts(Guid? id, CancellationToken cancellationToken)
        {
            PostsPageDto postsPageDto = await _postService.GetPosts(null, true, 5, default, true, p => p.UserId == id);

            return postsPageDto;
        }
    }
}
