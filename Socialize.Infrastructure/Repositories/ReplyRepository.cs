

using Microsoft.EntityFrameworkCore;
using Socialize.Core.Application.Repositories;
using Socialize.Core.Domain.Entities;
using Socialize.Infrastructure.Identity.Context;

namespace Socialize.Infrastructure.Identity.Repositories
{
	public class ReplyRepository : IReplyRepository
	{
		private readonly ApplicationDbContext _context;	
		private readonly DbSet<Reply> _replies;

		public ReplyRepository(ApplicationDbContext context)
		{
			_context = context;
			_replies = context.Replies;
		}
		public async Task<ICollection<Reply>> GetRepliesByCommentId(Guid commentId, CancellationToken cancellationToken)
		{
			IQueryable<Reply> query = _replies.Where(r => r.ParentCommentId == commentId).Include(r => r.User);
			return await query.ToListAsync(cancellationToken);
		}
	}
}
