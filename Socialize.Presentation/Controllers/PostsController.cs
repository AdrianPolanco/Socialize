using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Socialize.Core.Application.Dtos;
using Socialize.Core.Application.Services.Base;
using Socialize.Core.Application.Services.Interfaces;
using Socialize.Core.Application.UseCases.CreatePost;
using Socialize.Core.Application.UseCases.ReadPosts;
using Socialize.Core.Domain.Entities;
using Socialize.Infrastructure.Identity.Models;
using Socialize.Presentation.Enums;
using Socialize.Presentation.Extensions;
using Socialize.Presentation.Filters;
using Socialize.Presentation.Models.Posts;
using Socialize.Presentation.Services;
using System.Linq.Expressions;

namespace Socialize.Presentation.Controllers
{
    
    
    [Authorize]
    [RedirectToLoginIfNotAuthenticated]
    public class PostsController : Controller
    {
        private readonly ICreatePostUseCase _createPostUseCase;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly VideoValidator _videoValidator;
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IEntityService<Post> _postEntityService;

        public PostsController(ICreatePostUseCase createPostUseCase, UserManager<ApplicationUser> userManager, VideoValidator videoValidator, IPostService postService, IEntityService<Post> entityService, IMapper mapper)
        {
            _createPostUseCase = createPostUseCase;
            _userManager = userManager;
            _videoValidator = videoValidator;
            _postService = postService;  
            _postEntityService = entityService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(Guid? currentPageId, bool isNextPage)
        {
            MainViewModel mainViewModel = new MainViewModel
            {
                CurrentPageId = currentPageId,
                IsNextPage = isNextPage
            };
            return View(mainViewModel);
        }

        public async Task<IActionResult> Create(MainViewModel createPostViewModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return View("Index", createPostViewModel);

            string? attachmentUrl = default;
            Stream? _stream = null;

            if(createPostViewModel.Image is null && createPostViewModel.Mode is CreatePostModes.Image)
            {
                ModelState.AddModelError("Image", "The image is required.");
                return View("Index", createPostViewModel);
            }

            if(createPostViewModel.Image is not null && createPostViewModel.Mode == CreatePostModes.Image)
            {
                (Stream stream, string filename) = await createPostViewModel.Image.ConvertToStreamAsync(cancellationToken);

                _stream = stream;
                attachmentUrl = filename;
            }

            if(createPostViewModel.VideoUrl is not null && createPostViewModel.Mode == CreatePostModes.Video)
            {
                YoutubeResponses youtubeResponses = await _videoValidator.Validate(createPostViewModel.VideoUrl);
                if(youtubeResponses == YoutubeResponses.InvalidFormat)
                {
                    ModelState.AddModelError("VideoUrl", "Invalid format of video url. The expected format of the video is: \"https://www.youtube.com/embed/JWtnJjn6ng0\"");
                    return View("Index", createPostViewModel);
                }else if(youtubeResponses == YoutubeResponses.NotFound)
                {
                    ModelState.AddModelError("VideoUrl", "The video was not found. Please make sure the video exists and the url is correct.");
                    return View("Index", createPostViewModel);
                }
                attachmentUrl = createPostViewModel.VideoUrl;
            }   

            Guid userId = Guid.Parse(_userManager.GetUserId(User));
            attachmentUrl = createPostViewModel.Image is null && createPostViewModel.VideoUrl is null ? null : attachmentUrl;

            await _createPostUseCase.ExecuteAsync(userId, createPostViewModel.Content, cancellationToken, _stream, attachmentUrl);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
        {
            Expression<Func<Post, object>>[] includes = new Expression<Func<Post, object>>[]
            {
                p => p.User,
                p => p.Comments,
                p => p.Attachment
            };

            Post post = await _postEntityService.GetByIdAsync(id, cancellationToken, true, includes);

            PostDetailViewModel postDetailViewModel = _mapper.Map<PostDetailViewModel>(post);

            return View(postDetailViewModel);
        }
    }
}
