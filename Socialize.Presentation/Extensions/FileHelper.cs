namespace Socialize.Presentation.Extensions
{
    public static class FileHelper
    {
        public static async Task<(Stream fileStream, string fileName)> ConvertToStreamAsync(this IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is empty or null");
            }

            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream, cancellationToken);
            memoryStream.Position = 0; // Resetear la posicion del stream al comienzo

            return (memoryStream, file.FileName);
        }
    }
}
