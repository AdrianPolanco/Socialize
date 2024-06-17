
namespace Socialize.Core.Application.Services.Interfaces
{
    public interface IFileUploader
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder, CancellationToken cancellationToken);
    }
}
