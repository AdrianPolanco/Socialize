

namespace Socialize.Core.Application.Adapters
{
    public interface IUserManagerAdapter
    {
        Task<bool> UserExists(string username, CancellationToken cancellationToken);
        Task<bool> ResetPassword(string username, string password);
    }
}
