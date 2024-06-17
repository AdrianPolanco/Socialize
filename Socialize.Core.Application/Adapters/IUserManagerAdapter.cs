

namespace Socialize.Core.Application.Adapters
{
    public interface IUserManagerAdapter
    {
        Task<bool> UserExists(string username, CancellationToken cancellationToken);
    }
}
