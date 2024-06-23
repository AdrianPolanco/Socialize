

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Socialize.Core.Application.Repositories;
using Socialize.Core.Application.Services.Interfaces;
using Socialize.Core.Domain.Entities;
using Socialize.Core.Domain.Repositories.Base;
using Socialize.Infrastructure.Identity.Context;
using Socialize.Infrastructure.Identity.Models;
using Socialize.Infrastructure.Identity.Repositories;
using Socialize.Infrastructure.Identity.Repositories.Base;
using Socialize.Infrastructure.Identity.Services;
using System.Reflection;

namespace Socialize.Infrastructure.Identity.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // Configurar los DbContext usando PooledDbContextFactory
            services.AddDbContextPool<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityDatabase")));

            // Configurar Identity sin roles usando PooledDbContextFactory
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Configuraciones de opciones de Identity
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();

            //AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IRepository<User>, UserRepository>();
            services.AddScoped(typeof(IPartialRepository<>), typeof(PartialRepository<>));
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IReplyRepository, ReplyRepository>();

            //Services
            services.AddScoped<IFileUploader, FileUploader>();

            return services;
        }
    }
}
