using Socialize.Core.Application.Adapters;

namespace Socialize.Presentation.Adapters
{
    public class WebHostEnvironmentAdapter : IWebHostEnvironmentAdapter
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public WebHostEnvironmentAdapter(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string GetWebRootPath()
        {
            return _webHostEnvironment.WebRootPath;
        }
    }
}
