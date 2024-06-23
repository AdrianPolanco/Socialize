

using Microsoft.EntityFrameworkCore;
using Socialize.Core.Application.Repositories;
using Socialize.Core.Domain.Entities;
using Socialize.Infrastructure.Identity.Context;

namespace Socialize.Infrastructure.Identity.Repositories
{
	public class CommentRepository : ICommentRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly DbSet<Comment> _comments;

		public CommentRepository(ApplicationDbContext context)
		{
			_context = context;
			_comments = context.Comments;
		}

		public async Task<Comment> GetCommentById(Guid commentId, CancellationToken cancellationToken)
		{
			IQueryable<Comment> query = _comments.Include(c => c.User).Include(c => c.Replies);
			return await _comments.FirstOrDefaultAsync(c => c.Id == commentId, cancellationToken);
		}

		public async Task<ICollection<Comment>> GetCommentsByPostId(Guid postId, CancellationToken cancellationToken)
		{
			IQueryable<Comment> query = _comments.Include(c => c.User).Include(c => c.Replies);
			return await query.Where(c => c.PostId == postId).ToListAsync(cancellationToken);
		}
	}
}
