using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Socialize.Presentation.Filters;
using Socialize.Presentation.Models.Posts;

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

       /* public async Task<IActionResult> Create(CreatePostViewModel createPostViewModel)
        {
            if (!ModelState.IsValid) return View("Index", createPostViewModel);

            var post = _mapper.Map<CreatePostViewModel, Post>(createPostViewModel);
            post.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            post.CreatedAt = DateTime.Now;

            await _createPostUseCase.Execute(post);

            return RedirectToAction("Index");
        }*/
    }
}
