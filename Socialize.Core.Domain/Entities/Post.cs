using Socialize.Core.Domain.Entities.Base;

namespace Socialize.Core.Domain.Entities
{
    public class Post : Entity
    {
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public List<Comment> Comments { get; set; }
    }

}
