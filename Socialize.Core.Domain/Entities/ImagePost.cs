using Socialize.Core.Domain.Entities.Base;

namespace Socialize.Core.Domain.Entities
{
    public class ImagePost: BasePost
    {
        public string? Content { get; set; }    
        public string ImageUrl { get; set; }
    }
}
