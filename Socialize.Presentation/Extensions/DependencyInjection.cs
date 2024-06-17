using Socialize.Core.Application.Adapters;
using Socialize.Infrastructure.Identity.Adapters;
using Socialize.Presentation.Adapters;

namespace Socialize.Presentation.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Program).Assembly);
            services.AddScoped<IWebHostEnvironmentAdapter, WebHostEnvironmentAdapter>();
            services.AddScoped<IUserManagerAdapter, UserManagerAdapter>();
            return services;
        }
    }
}
