

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Socialize.Infrastructure.Identity.Context;
using Socialize.Infrastructure.Identity.Models;

namespace Socialize.Infrastructure.Identity.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // Configurar los DbContext usando PooledDbContextFactory
            services.AddPooledDbContextFactory<ApplicationIdentityDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityDatabase")));

            // Configurar Identity sin roles usando PooledDbContextFactory
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                // Configuraciones de opciones de Identity
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();

            // Registrar el servicio de DbContext manualmente para resolver el servicio en tiempo de diseño
            services.AddTransient(provider =>
            {
                var dbContextFactory = provider.GetRequiredService<IDbContextFactory<ApplicationIdentityDbContext>>();
                return dbContextFactory.CreateDbContext();
            });

            return services;
        }
    }
}
