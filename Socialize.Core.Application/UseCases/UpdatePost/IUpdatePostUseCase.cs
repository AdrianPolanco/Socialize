
namespace Socialize.Core.Application.UseCases.UpdatePost
{
    public interface IUpdatePostUseCase
    {
        Task<bool> ExecuteAsync(Guid postId, string content, CancellationToken cancellationToken, Stream? stream = null, string? name = null);
    }
}
