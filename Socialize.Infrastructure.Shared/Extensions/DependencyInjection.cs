using Microsoft.Extensions.DependencyInjection;
using Socialize.Core.Application.Services.Interfaces;
using Socialize.Infrastructure.Shared.Services;

namespace Socialize.Infrastructure.Shared.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddShared(this IServiceCollection services)
        {
            services.AddTransient<IEmailSender, EmailSender>();
            return services;
        }
    }
}
