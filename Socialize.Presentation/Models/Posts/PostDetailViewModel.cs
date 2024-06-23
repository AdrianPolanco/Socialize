using Google.Apis.YouTube.v3.Data;
using System.ComponentModel.DataAnnotations;

namespace Socialize.Presentation.Models.Posts
{
    public class PostDetailViewModel
    {
        [Required]
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedAtFormatted { get; set; }
        public string Username { get; set; }
        public string UsernamePhoto { get; set; }
        public string? AttachmentUrl { get; set; }
        public int CommentsCount { get; set; }
        public List<Comment> Comments { get; set; }
        [Required]
        public Comment NewComment { get; set; }
    }
}
