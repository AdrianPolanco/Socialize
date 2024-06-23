using Microsoft.Extensions.DependencyInjection;
using Socialize.Core.Application.Services;
using Socialize.Core.Application.Services.Base;
using Socialize.Core.Application.Services.Interfaces;
using Socialize.Core.Application.UseCases.CreateNewUser;
using Socialize.Core.Application.UseCases.CreatePost;
using Socialize.Core.Application.UseCases.ReadPosts;
using Socialize.Core.Application.UseCases.ResetPassword;
using Socialize.Core.Application.UseCases.UpdatePost;
using System.Reflection;

namespace Socialize.Core.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped(typeof(IFileService<>), typeof(FileService<>));
            services.AddScoped(typeof(IEntityService<>), typeof(EntityService<>));
            services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
            services.AddScoped<IResetPasswordUseCase, ResetPasswordUseCase>();
            services.AddScoped<ICreatePostUseCase, CreatePostUseCase>();
            services.AddScoped<IUpdatePostUseCase, UpdatePostUseCase>();
            services.AddScoped<IReadPostsUseCase, ReadPostsUseCase>();

            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentService>();
            return services;
        }
    }
}
