using Socialize.Core.Domain.Entities.Base;

namespace Socialize.Core.Application.Services.Interfaces
{
    public interface IFileService<T> where T : Entity
    {
        Task<string> UploadImageAsync(Stream fileStream, string fileName, string username, CancellationToken cancellationToken);
        string GetDefaultImageRoute();
        void DeleteFolder(Guid id);
    }
}
