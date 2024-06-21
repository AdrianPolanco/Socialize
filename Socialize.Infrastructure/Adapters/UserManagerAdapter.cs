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

        public async Task<bool> ResetPassword(string username, string newPassword)
        {
            ApplicationUser applicationUser = await _userManager.FindByNameAsync(username);
            if (applicationUser == null) return false;
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);
            IdentityResult result = await _userManager.ResetPasswordAsync(applicationUser, resetToken, newPassword);
            return result.Succeeded;
        }
    }
}
