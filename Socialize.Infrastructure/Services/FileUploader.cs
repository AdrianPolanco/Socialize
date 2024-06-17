using Socialize.Core.Application.Adapters;
using Socialize.Core.Application.Services.Interfaces;

namespace Socialize.Infrastructure.Identity.Services
{
    public class FileUploader : IFileUploader
    {
        private readonly IWebHostEnvironmentAdapter _env;

        public FileUploader(IWebHostEnvironmentAdapter env)
        {
            _env = env;
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder, CancellationToken cancellationToken)
        {
            if (fileStream == null || fileStream.Length == 0)
            {
                throw new ArgumentException("File stream is empty");
            }

            string wwwRootPath = _env.GetWebRootPath();
            //Combinamos la carpeta con el path de la carpeta wwwroot
            string folderPath = $"{wwwRootPath}/{folder}";

            //Si la carpeta no existe, la creamos
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);


            //Combinamos la carpeta con el nombre del archivo
            string filePath = $"{folderPath}/{fileName}";

            //Guardamos el archivo en la carpeta determinada
            using (var fileStreamToSave = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(fileStreamToSave, cancellationToken);
            }

            //Retornamos el path del archivo
            return $"{folder}/{fileName}";
        }
    }
}
