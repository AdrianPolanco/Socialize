using Socialize.Core.Domain.Enums;

namespace Socialize.Presentation.Models.Posts
{
    public class PostViewModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedAtFormatted { get; set; }
        public string Username { get; set; }
        public string UsernamePhoto { get; set; }
        public AttachmentTypes? Type { get; set; }
        public int CommentsCount { get; set; }
        public string? AttachmentUrl { get; set; }
    }
}
