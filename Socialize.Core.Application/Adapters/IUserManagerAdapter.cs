

using Socialize.Core.Domain.Entities;

namespace Socialize.Core.Application.Adapters
{
    public interface IUserManagerAdapter
    {
        Task<bool> UserExists(string username, CancellationToken cancellationToken);
        Task HashPassword(Guid userId, string password);
        Task<User> UpdateUser(User user);
        Task<bool> ResetPassword(string username, string password);
    }
}
