using Socialize.Core.Domain.Entities;


namespace Socialize.Core.Application.UseCases.CreatePost
{
    public interface ICreatePostUseCase
    {
        Task<bool> ExecuteAsync(Guid userId, string content, CancellationToken cancellationToken, Stream? stream = null, string? name = null);
    }
}
