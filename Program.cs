// Program.cs - application entry point and middleware pipeline.

var builder = WebApplication.CreateBuilder(args);

// Register MVC (controllers + Razor views) with the dependency-injection container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// In production, show a friendly error page instead of a stack trace.
// (In Development, ASP.NET Core shows the detailed developer exception page automatically.)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// For unknown URLs (404) and other status codes, re-run /Home/Error?code=404
// so users see our styled page rather than a blank browser error.
app.UseStatusCodePagesWithReExecute("/Home/Error", "?code={0}");

app.UseStaticFiles();   // serves wwwroot (css, js, images)
app.UseRouting();
app.UseAuthorization();

// Default route:  /{controller}/{action}/{id?}   ->   starts on Home/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
