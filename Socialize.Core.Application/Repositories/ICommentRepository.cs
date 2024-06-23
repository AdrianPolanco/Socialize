

using Socialize.Core.Domain.Entities;

namespace Socialize.Core.Application.Repositories
{
	public interface ICommentRepository
	{
		Task<ICollection<Comment>> GetCommentsByPostId(Guid postId, CancellationToken cancellationToken);
		Task<Comment> GetCommentById(Guid commentId, CancellationToken cancellationToken);
	}
}
