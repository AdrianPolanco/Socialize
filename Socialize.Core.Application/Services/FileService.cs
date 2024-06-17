using Socialize.Core.Application.Adapters;
using Socialize.Core.Application.Services.Interfaces;
using Socialize.Core.Domain.Entities.Base;

namespace Socialize.Core.Application.Services
{
    public class FileService<T> : IFileService<T> where T : Entity
    {
        private readonly IFileUploader _fileUploader;
        private readonly IWebHostEnvironmentAdapter _webHostEnvironmentAdapter;
        private readonly string _folder;
        private readonly string _baseFolder;

        public FileService(IFileUploader fileUploader, IWebHostEnvironmentAdapter webHostEnvironmentAdapter)
        {
            _fileUploader = fileUploader;
            _webHostEnvironmentAdapter = webHostEnvironmentAdapter;
            _baseFolder = $"/images";
            _folder = $"{_baseFolder}/{typeof(T).Name}s";
        }

        public string GetDefaultImageRoute()
        {
            string defaultImageRoute = $"{_baseFolder}/user-default.png";
            return defaultImageRoute;
        }

        public async Task<string> UploadImageAsync(Stream fileStream, string fileName, string username, CancellationToken cancellationToken)
        {
            if (fileStream == null || fileStream.Length == 0)
            {
                throw new ArgumentException("File stream is empty");
            }

            string folder = $"{_folder}/{username}";
            return await _fileUploader.UploadFileAsync(fileStream, fileName, folder, cancellationToken);
        }

        public void DeleteFolder(Guid id)
        {
            // Construct the folder path
            string folderPath = $"{_webHostEnvironmentAdapter.GetWebRootPath()}/{_folder}/{id}";

            if (Directory.Exists(folderPath))
            {
                // Borra la carpeta con el id y todo su contenido
                Directory.Delete(folderPath, true);
            }
        }
    }
}

