

using Socialize.Core.Application.Repositories;
using Socialize.Core.Application.Services.Interfaces;
using Socialize.Core.Domain.Entities;

namespace Socialize.Core.Application.Services
{
	public class CommentService : ICommentService
	{
		private readonly ICommentRepository _commentRepository;
		public CommentService(ICommentRepository commentRepository)
		{
			_commentRepository = commentRepository;
		}

		public async Task<Comment> GetCommentById(Guid commentId, CancellationToken cancellationToken)
		{
			return await _commentRepository.GetCommentById(commentId, cancellationToken);
		}

		public async Task<ICollection<Comment>> GetCommentsByPostId(Guid postId, CancellationToken cancellationToken)
		{
			return await _commentRepository.GetCommentsByPostId(postId, cancellationToken);
		}
	}
}
