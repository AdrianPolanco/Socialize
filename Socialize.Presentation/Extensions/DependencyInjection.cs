namespace Socialize.Presentation.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Program).Assembly);
            return services;
        }
    }
}
