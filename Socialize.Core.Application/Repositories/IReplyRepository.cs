

using Socialize.Core.Domain.Entities;

namespace Socialize.Core.Application.Repositories
{
	public interface IReplyRepository
	{
		public Task<ICollection<Reply>> GetRepliesByCommentId(Guid commentId, CancellationToken cancellationToken);
	}
}
