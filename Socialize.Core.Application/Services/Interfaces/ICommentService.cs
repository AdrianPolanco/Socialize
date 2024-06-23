

using Socialize.Core.Domain.Entities;

namespace Socialize.Core.Application.Services.Interfaces
{
	public interface ICommentService
	{
		public Task<ICollection<Comment>> GetCommentsByPostId(Guid postId, CancellationToken cancellationToken);
		public Task<Comment> GetCommentById(Guid commentId, CancellationToken cancellationToken);
		Task<ICollection<Reply>> GetRepliesAsync(Guid commentId, CancellationToken cancellationToken);
		Task ReplyAsync(Guid parentCommentId, Guid userId, string content, CancellationToken cancellationToken);
	}
}
