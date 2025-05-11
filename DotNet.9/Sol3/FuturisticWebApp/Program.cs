using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Define routes
app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("<html><body style='background-color: #001f3f; color: #FF4500; font-family: Arial, sans-serif;'>\n<h1>Welcome to the Futuristic Web App</h1>\n<p>This is the main landing page.</p>\n</body></html>");
});

app.MapGet("/about", async context =>
{
    await context.Response.WriteAsync("<html><body style='background-color: #001f3f; color: #FF4500; font-family: Arial, sans-serif;'>\n<h1>About Me</h1>\n<p>This page contains information about me.</p>\n</body></html>");
});

app.MapGet("/links", async context =>
{
    await context.Response.WriteAsync("<html><body style='background-color: #001f3f; color: #FF4500; font-family: Arial, sans-serif;'>\n<h1>Links to My Flipboard Magazines</h1>\n<p>Here are some links to my Flipboard magazines.</p>\n</body></html>");
});

app.Run();
