using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Socialize.Presentation.Filters;

namespace Socialize.Presentation.Controllers
{
    
    
    [Authorize]
    [RedirectToLoginIfNotAuthenticated]
    public class PostsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
