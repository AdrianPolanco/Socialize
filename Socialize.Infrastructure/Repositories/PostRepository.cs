

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Socialize.Core.Application.Dtos;
using Socialize.Core.Application.Repositories;
using Socialize.Core.Domain.Entities;
using Socialize.Infrastructure.Identity.Context;
using System.Linq.Expressions;

namespace Socialize.Infrastructure.Identity.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Post> _posts;
        private readonly IMapper _mapper;

        public PostRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _posts = context.Posts;
            _mapper = mapper;
        }

        public async Task<PostsPageDto> GetPosts(GetPostsDto getPostsDto, CancellationToken cancellationToken, bool readOnly, Expression<Func<Post, bool>> filter = null)
        {
            List<Post> posts = await _posts.Include(p => p.User).Include(p => p.Comments).Include(p => p.Attachment).Where(filter).OrderByDescending(p => p.CreatedAt).ToListAsync(cancellationToken);
            List<PostDto> postsDto = _mapper.Map<List<PostDto>>(posts);
            return new PostsPageDto(postsDto, null, null, true);
        }
    }
}
