using Microsoft.AspNetCore.Identity;
using Socialize.Core.Application.Adapters;
using Socialize.Infrastructure.Identity.Models;

namespace Socialize.Infrastructure.Identity.Adapters
{
    public class UserManagerAdapter : IUserManagerAdapter
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagerAdapter(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<bool> UserExists(string username, CancellationToken cancellationToken)
        {
            ApplicationUser applicationUser = await _userManager.FindByNameAsync(username);
            return applicationUser != null;
        }
    }
}
