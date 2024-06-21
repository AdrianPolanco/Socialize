using Google.Apis.YouTube.v3;
using Socialize.Presentation.Enums;
using System.Text.RegularExpressions;

namespace Socialize.Presentation.Services
{
    public class VideoValidator
    {
        public VideoValidator(YouTubeService youtubeService)
        {
            _youtubeService = youtubeService;
        }
        private readonly YouTubeService _youtubeService;

        public async Task<YoutubeResponses> Validate(string videoUrl)
        {
            var searchRequest = _youtubeService.Videos.List("snippet");

            if (!IsValidFormatUrl(videoUrl)) return YoutubeResponses.InvalidFormat;

            searchRequest.Id = ExtractVideoId(videoUrl);

            var searchResponse = await searchRequest.ExecuteAsync();

            if (searchResponse.Items.Count == 0) return YoutubeResponses.NotFound;

            return YoutubeResponses.Success;
        }
        public bool IsValidFormatUrl(string videoUrl)
        {
            string pattern = @"https://www\.youtube\.com/embed/[A-Za-z0-9]";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(videoUrl);
        }
        public string ExtractVideoId(string videoUrl)
        {
            // Dividir la URL por las barras inclinadas
            string[] parts = videoUrl.Split('/');

            // El ID del video estará en el último segmento de la URL
            string videoId = parts[parts.Length - 1];

            return videoId;
        }

    }
}
