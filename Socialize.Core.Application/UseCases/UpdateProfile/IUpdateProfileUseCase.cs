using Socialize.Core.Domain.Entities;


namespace Socialize.Core.Application.UseCases.UpdateProfile
{
    public interface IUpdateProfileUseCase
    {
        Task<bool> ExecuteAsync(User user, Stream stream, string fileName, CancellationToken cancellationToken);
    }
}
