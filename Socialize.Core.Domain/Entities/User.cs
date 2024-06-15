using Socialize.Core.Domain.Entities.Base;
using Socialize.Core.Domain.ValueObjects;

namespace Socialize.Core.Domain.Entities
{
    public class User : Entity
    {
        public string Username { get; set; }
        public Email Email { get; set; }
        public PhoneNumber PhoneNumber { get; set; }
        public bool IsActived { get; set; }
        public List<Friendship> Friendships { get; set; }
        public List<Post> Posts { get; set; }
        public List<VideoPost> VideoPosts { get; set; }
        public List<ImagePost> ImagePosts { get; set; }
    }
}
