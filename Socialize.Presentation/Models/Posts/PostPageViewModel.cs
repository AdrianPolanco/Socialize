using Socialize.Core.Application.Dtos;

namespace Socialize.Presentation.Models.Posts
{
    public class PostPageViewModel
    {
        public ICollection<PostViewModel> Posts { get; set; }
        public Guid? NextId { get; set; }
        public Guid? PreviousId { get; set; }
        public bool IsFirstPage { get; set; }
    }
}
