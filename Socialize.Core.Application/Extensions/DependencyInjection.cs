using Microsoft.Extensions.DependencyInjection;
using Socialize.Core.Application.Services;
using Socialize.Core.Application.Services.Base;
using Socialize.Core.Application.Services.Interfaces;
using Socialize.Core.Application.UseCases.CreateNewUser;
using Socialize.Core.Application.UseCases.ResetPassword;

namespace Socialize.Core.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped(typeof(IFileService<>), typeof(FileService<>));
            services.AddScoped(typeof(IEntityService<>), typeof(EntityService<>));
            services.AddScoped<ICreateNewUserUseCase, CreateNewUserUseCase>();
            services.AddScoped<IResetPasswordUseCase, ResetPasswordUseCase>();
            return services;
        }
    }
}
