

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Socialize.Core.Domain.Repositories.Base;
using Socialize.Infrastructure.Identity.Context;
using Socialize.Infrastructure.Identity.Models;
using Socialize.Infrastructure.Identity.Repositories.Base;
using System.Reflection;

namespace Socialize.Infrastructure.Identity.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // Configurar los DbContext usando PooledDbContextFactory
            services.AddPooledDbContextFactory<ApplicationDbContext>(options =>
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
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Registrar el servicio de DbContext manualmente para resolver el servicio en tiempo de diseño
            services.AddTransient(provider =>
            {
                var dbContextFactory = provider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
                return dbContextFactory.CreateDbContext();
            });

            //AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IPartialRepository<>), typeof(PartialRepository<>));

            return services;
        }
    }
}
