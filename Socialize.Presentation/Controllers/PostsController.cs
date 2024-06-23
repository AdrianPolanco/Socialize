using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Socialize.Core.Application.Services.Base;
using Socialize.Core.Application.Services.Interfaces;
using Socialize.Core.Application.UseCases.CreatePost;
using Socialize.Core.Domain.Entities;
using Socialize.Infrastructure.Identity.Models;
using Socialize.Presentation.Enums;
using Socialize.Presentation.Extensions;
using Socialize.Presentation.Filters;
using Socialize.Presentation.Models.Comments;
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
        private readonly ICommentService _commentService;

        public PostsController(ICreatePostUseCase createPostUseCase, UserManager<ApplicationUser> userManager, VideoValidator videoValidator, 
            IPostService postService, IEntityService<Post> entityService, IMapper mapper, IEntityService<Comment> _commentEntityService,
            ICommentService commentService)
        {
            _createPostUseCase = createPostUseCase;
            _userManager = userManager;
            _videoValidator = videoValidator;
            _postService = postService;  
            _postEntityService = entityService;
            _mapper = mapper;
            _commentEntityService = _commentEntityService;
            _commentService = commentService;
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
            Expression<Func<Post, object>>[] includesPost = new Expression<Func<Post, object>>[]
            {
                p => p.User,
                p => p.Attachment
            };

            ICollection<Comment> commentsCollection = await _commentService.GetCommentsByPostId(id, cancellationToken);
            List<Comment> comments = commentsCollection.ToList();

            Post post = await _postEntityService.GetByIdAsync(id, cancellationToken, true, includesPost);

            PostDetailViewModel postDetailViewModel = _mapper.Map<PostDetailViewModel>(post);

            postDetailViewModel.Comments = comments;
            postDetailViewModel.CommentsCount = comments.Count;

            return View(postDetailViewModel);
        }

        public async Task<IActionResult> Comment(Guid postId, string content, CancellationToken cancellationToken)
        {
			Guid userId = Guid.Parse(_userManager.GetUserId(User));

			await _postService.CommentAsync(postId, userId, content, cancellationToken);

			return RedirectToAction("Details", new { id = postId });
		}

        public async Task<IActionResult> CommentDetails(Guid id, CancellationToken cancellationToken)
        {
			Guid currentUserId = Guid.Parse(_userManager.GetUserId(User));

            Comment comment = await _commentService.GetCommentById(id, cancellationToken);

            CommentViewModel commentViewModel = _mapper.Map<CommentViewModel>(comment);

            ICollection<Reply> repliesCollection = await _commentService.GetRepliesAsync(id, cancellationToken);
            List<Reply> replies = repliesCollection.ToList();

            commentViewModel.Replies = replies;
            commentViewModel.RepliesCount = replies.Count;

            return View(commentViewModel);
		}

        public async Task<IActionResult> Reply(Guid commentId, string content, CancellationToken cancellationToken)
        {
            Guid userId = Guid.Parse(_userManager.GetUserId(User));

			await _commentService.ReplyAsync(commentId, userId, content, cancellationToken);

			return RedirectToAction("CommentDetails", new { id = commentId });
        }
    }
}
