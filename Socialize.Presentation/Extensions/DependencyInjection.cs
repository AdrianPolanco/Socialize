using Socialize.Core.Application.Adapters;
using Socialize.Infrastructure.Identity.Adapters;
using Socialize.Presentation.Adapters;
using Socialize.Presentation.Filters;
using Socialize.Presentation.Middlewares;

namespace Socialize.Presentation.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Home/Index";
                options.AccessDeniedPath = "/Home/Index";
                options.ReturnUrlParameter = "";
            });
            services.AddAutoMapper(typeof(Program).Assembly);
            services.AddScoped<IWebHostEnvironmentAdapter, WebHostEnvironmentAdapter>();
            services.AddScoped<IUserManagerAdapter, UserManagerAdapter>();
            services.AddScoped<RedirectToPostsFilterAttribute>();
            services.AddScoped<RedirectToLoginIfNotAuthenticatedAttribute>();
            services.AddSession();
            return services;
        }
    }
}
