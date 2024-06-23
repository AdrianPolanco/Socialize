

using Socialize.Core.Application.Repositories;
using Socialize.Core.Application.Services.Base;
using Socialize.Core.Application.Services.Interfaces;
using Socialize.Core.Domain.Entities;

namespace Socialize.Core.Application.Services
{
	public class CommentService : ICommentService
	{
		private readonly ICommentRepository _commentRepository;
		private readonly IReplyRepository _replyRepository;
		private readonly IEntityService<Reply> _entityService;	

		public CommentService(ICommentRepository commentRepository, IReplyRepository repository, IEntityService<Reply> entityService)
		{
			_commentRepository = commentRepository;
			_replyRepository = repository;
			_entityService = entityService;
		}

		public async Task<Comment> GetCommentById(Guid commentId, CancellationToken cancellationToken)
		{
			return await _commentRepository.GetCommentById(commentId, cancellationToken);
		}

		public async Task<ICollection<Comment>> GetCommentsByPostId(Guid postId, CancellationToken cancellationToken)
		{
			return await _commentRepository.GetCommentsByPostId(postId, cancellationToken);
		}

		public async Task<ICollection<Reply>> GetRepliesAsync(Guid commentId, CancellationToken cancellationToken)
		{
			return await _replyRepository.GetRepliesByCommentId(commentId, cancellationToken);
		}

		public async Task ReplyAsync(Guid parentCommentId, Guid userId, string content, CancellationToken cancellationToken)
		{
			Reply newReply = new()
			{
				Content = content,
				ParentCommentId = parentCommentId,
				UserId = userId,
				CreatedAt = DateTime.Now
			};

			await _entityService.AddAsync(newReply, cancellationToken);
		}
	}
}
