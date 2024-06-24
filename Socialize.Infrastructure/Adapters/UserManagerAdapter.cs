using Microsoft.AspNetCore.Identity;
using Socialize.Core.Application.Adapters;
using Socialize.Core.Domain.Entities;
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

        public async Task HashPassword(Guid userId, string password)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId.ToString());
            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, password);
        }

        public async Task<User> UpdateUser(User user)
        {
            ApplicationUser fetchedUser = await _userManager.FindByIdAsync(user.Id.ToString());
            fetchedUser.Name = user.Name;
            fetchedUser.Lastname = user.Lastname;
            fetchedUser.Email = user.Email.Value;
            fetchedUser.PhoneNumber = user.PhoneNumber.Value;
            fetchedUser.PhotoUrl = user.PhotoUrl;

            await _userManager.UpdateAsync(fetchedUser);

            return user;
        }
    }
}
