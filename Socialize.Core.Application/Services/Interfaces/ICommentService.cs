

using Socialize.Core.Domain.Entities;

namespace Socialize.Core.Application.Services.Interfaces
{
	public interface ICommentService
	{
		public Task<ICollection<Comment>> GetCommentsByPostId(Guid postId, CancellationToken cancellationToken);
		public Task<Comment> GetCommentById(Guid commentId, CancellationToken cancellationToken);
	}
}
