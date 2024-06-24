using Socialize.Core.Domain.Entities;

namespace Socialize.Presentation.Models.Friendships
{
    public class FriendshipSearchViewModel
    {
        public string? Username { get; set; }
        public List<User> Users { get; set; }
        public List<User> Friends { get; set; }
        public bool Searched { get; set; }
    }
}
