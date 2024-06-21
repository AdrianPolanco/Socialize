
using Socialize.Core.Application.Services.Base;
using Socialize.Core.Application.Services.Interfaces;
using Socialize.Core.Domain.Entities;
using Socialize.Core.Domain.Enums;

namespace Socialize.Core.Application.UseCases.CreatePost
{
    public class CreatePostUseCase : ICreatePostUseCase
    {
        private readonly IEntityService<User> _userService;
        private readonly IEntityService<Post> _postService;
        private readonly IFileService<Post> _fileService;

        public CreatePostUseCase(IEntityService<User> userService, IEntityService<Post> postService, IFileService<Post> fileService)
        {
            _userService = userService;
            _postService = postService;
            _fileService = fileService;
        }

        public async Task<bool> ExecuteAsync(Guid userId, string content, CancellationToken cancellationToken, Stream? stream = null, string? name = null)
        {
            if (userId == Guid.Empty) return false;
            if(string.IsNullOrWhiteSpace(content)) return false;

            var user = await _userService.GetByIdAsync(userId, cancellationToken, true);

            if (user == null) return false;

            Attachment? attachment = null;

            if (stream is null && !string.IsNullOrWhiteSpace(name)) attachment = new Attachment { Url = name, Type = AttachmentTypes.Video };
            else if (stream is not null && !string.IsNullOrWhiteSpace(name)) {
                attachment = new Attachment { Type = AttachmentTypes.Image };

                var filePath = await _fileService.UploadImageAsync(stream, name, user.Username,cancellationToken);

                attachment.Url = filePath;
            } 

            var post = new Post
            {
                UserId = user.Id,
                Content = content,
                Attachment = attachment,
                CreatedAt = DateTime.Now,
            };

            await _postService.AddAsync(post, cancellationToken);
            return true;
        }
    }
}
