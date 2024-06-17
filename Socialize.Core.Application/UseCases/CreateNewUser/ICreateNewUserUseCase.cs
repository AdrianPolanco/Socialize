
using Socialize.Core.Domain.Entities;

namespace Socialize.Core.Application.UseCases.CreateNewUser
{
    public interface ICreateNewUserUseCase
    {
        public Task<bool> ExecuteAsync(User user, Stream stream, string fileName, CancellationToken cancellationToken);
    }
}
