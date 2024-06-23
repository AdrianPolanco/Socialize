


using Socialize.Core.Application.Services.Base;
using Socialize.Core.Application.Services.Interfaces;
using Socialize.Core.Domain.Entities;
using Socialize.Core.Domain.Enums;
using System.Linq.Expressions;

namespace Socialize.Core.Application.UseCases.UpdatePost
{
    public class UpdatePostUseCase : IUpdatePostUseCase
    {
        private readonly IEntityService<Post> _postService;
        private readonly IEntityService<Attachment> _attachmentService;
        private readonly IFileService<Post> _fileService;

        public UpdatePostUseCase(IEntityService<User> userService, IEntityService<Post> postService, IFileService<Post> fileService, IEntityService<Attachment> attachmentService)
        {
            _postService = postService;
            _fileService = fileService;
            _attachmentService = attachmentService;
        }

        public async Task<bool> ExecuteAsync(Guid postId, string content, CancellationToken cancellationToken, Stream? stream = null, string? name = null)
        {
            if (postId == Guid.Empty) return false;
            if (string.IsNullOrWhiteSpace(content)) return false;

            Expression<Func<Post, object>>[] includesPost = new Expression<Func<Post, object>>[]
             {
                    p => p.User,
                    p => p.Attachment
             };

            var post = await _postService.GetByIdAsync(postId, cancellationToken, false, true, includesPost);

            if (post == null) return false;

            post.Content = content;


            if (stream is null && !string.IsNullOrWhiteSpace(name)) {
                if(post.Attachment is not null)
                {
                    /*Attachment currentAttachment = await _attachmentService.GetByIdAsync(post.Attachment.Id, cancellationToken);
                    currentAttachment.Url = name;
                    currentAttachment.Type = AttachmentTypes.Video;*/
                    post.Attachment.Url = name;
                    post.Attachment.Type = AttachmentTypes.Video;
                    post.Attachment.PostId = post.Id;
                    if(post.Attachment.Deleted) post.Attachment.Deleted = false;
                    await _postService.UpdateAsync(post, cancellationToken);    
                }
                else {                     
                    Attachment attachment = new Attachment
                    {
                        Url = name,
                        Type = AttachmentTypes.Video,
                        PostId = post.Id
                    };
                   // if (post.Attachment is not null) await _attachmentService.DeleteAsync(post.Attachment.Id, cancellationToken);
                    await _attachmentService.AddAsync(attachment, cancellationToken);
                 }         
            }else if (stream is not null && !string.IsNullOrWhiteSpace(name))
            {
                var filePath = await _fileService.UploadImageAsync(stream, name, post.User.Username, cancellationToken);

                if(post.Attachment is not null)
                {
                  /*  Attachment currentAttachment = await _attachmentService.GetByIdAsync(post.Attachment.Id, cancellationToken);
                    currentAttachment.Url = filePath;
                    currentAttachment.Type = AttachmentTypes.Image;*/
                    post.Attachment.Url = filePath;
                    post.Attachment.Type = AttachmentTypes.Image;
                    post.Attachment.PostId = post.Id;
                    if(post.Attachment.Deleted) post.Attachment.Deleted = false;    
                    await _postService.UpdateAsync(post, cancellationToken);
                }
                else
                {
                    Attachment attachment = new Attachment
                    {
                        Url = filePath,
                        Type = AttachmentTypes.Image,
                        PostId = post.Id
                    };
                 //   if(post.Attachment is not null) await _attachmentService.DeleteAsync(post.Attachment.Id, cancellationToken);
                    await _attachmentService.AddAsync(attachment, cancellationToken);
                }
                
            }
            else
            {
                await _attachmentService.DeleteAsync(post.Attachment.Id, cancellationToken);
            }

            post.CreatedAt = post.CreatedAt;

            await _postService.UpdateAsync(post, cancellationToken);
            return true;
        }
    }
}
