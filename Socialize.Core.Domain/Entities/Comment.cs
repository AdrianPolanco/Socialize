

using Socialize.Core.Domain.Entities.Base;

namespace Socialize.Core.Domain.Entities
{
    public class Comment : Entity
    {
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public List<Reply> Replies { get; set; }

    }

}
