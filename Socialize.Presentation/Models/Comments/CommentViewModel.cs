using Socialize.Core.Domain.Entities;
using Socialize.Core.Domain.Enums;

namespace Socialize.Presentation.Models.Comments
{
    public class CommentViewModel
    {
		public Guid Id { get; set; }
		public string Content { get; set; }
		public DateTime CreatedAt { get; set; }
		public string CreatedAtFormatted { get; set; }
		public string Username { get; set; }
		public string UsernamePhoto { get; set; }
		public int RepliesCount { get; set; }
		public Guid PostId { get; set; }
		public List<Reply> Replies { get; set; }
	}
}
