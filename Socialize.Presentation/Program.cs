
using Socialize.Core.Application.Extensions;
using Socialize.Infrastructure.Identity.Extensions;
using Socialize.Infrastructure.Shared.Services.Interfaces;
using Socialize.Infrastructure.Shared.Services;
using Socialize.Presentation.Extensions;
using Socialize.Infrastructure.Shared.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddApplication();
builder.Services.AddIdentityPersistence(builder.Configuration);
builder.Services.AddPresentation();
// Configuraci�n de Shared
builder.Services.Configure<GoogleSettings>(builder.Configuration.GetSection("GoogleSettings"));
builder.Services.AddTransient<GoogleService>();
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
