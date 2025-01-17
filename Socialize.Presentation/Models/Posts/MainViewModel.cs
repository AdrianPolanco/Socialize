﻿using Socialize.Presentation.Enums;
using System.ComponentModel.DataAnnotations;

namespace Socialize.Presentation.Models.Posts
{
    public class MainViewModel
    {
        [Required(ErrorMessage = "Posts must have content!")]
        public string Content { get; set; }
        public CreatePostModes Mode { get; set; }

        public IFormFile? Image { get; set; }
        public string? VideoUrl { get; set; }
        public Guid? CurrentPageId { get; set; }
        public bool IsNextPage { get; set; }
    }
}
