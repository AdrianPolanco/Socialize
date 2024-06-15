

namespace Socialize.Core.Domain.Entities.Base
{
    public abstract class BasePost: Entity
    {
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
