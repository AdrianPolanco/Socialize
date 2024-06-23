using Socialize.Core.Domain.Entities;
using Socialize.Presentation.Enums;
using System.ComponentModel.DataAnnotations;

namespace Socialize.Presentation.Models.Posts
{
    public class EditPostViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Posts must have content!")]
        public string Content { get; set; }
        public CreatePostModes Mode { get; set; }

        public IFormFile? Image { get; set; }
        public string? VideoUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
        public Attachment? Attachment { get; set; }
    }
}
