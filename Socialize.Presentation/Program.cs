
using Socialize.Core.Application.Extensions;
using Socialize.Infrastructure.Identity.Extensions;
using Socialize.Infrastructure.Shared.Services.Interfaces;
using Socialize.Infrastructure.Shared.Services;
using Socialize.Presentation.Extensions;
using Socialize.Infrastructure.Shared.Settings;
using Socialize.Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddApplication();
builder.Services.AddIdentityPersistence(builder.Configuration);
builder.Services.AddPresentation();
// Configuración de Shared
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddTransient<IEmailSender, EmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

/*app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 403)
    {
        context.Request.Path = "/Home/Index";
        context.Response.StatusCode = 200;
        context.Request.QueryString = new QueryString("?AccessDenied=true");
    }
});
*/
//app.UseMiddleware<RedirectToHomeMiddleware>();

app.Run();
