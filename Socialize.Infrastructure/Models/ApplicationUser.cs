

using Microsoft.AspNetCore.Identity;

namespace Socialize.Infrastructure.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string PhotoUrl { get; set; }
        public bool Deleted { get; set; }   
    }
}
