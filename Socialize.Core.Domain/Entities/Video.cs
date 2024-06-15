using Socialize.Core.Domain.Entities.Base;

namespace Socialize.Core.Domain.Entities
{
    public class VideoPost: BasePost
    {
        public string? Content { get; set; }    
        public string VideoUrl { get; set; }
    }
}
