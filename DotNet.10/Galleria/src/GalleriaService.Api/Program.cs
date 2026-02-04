using GalleriaService.Api.Features;
using GalleriaService.Api.Features.FileManagement;
using GalleriaService.Api.Features.Gallery;
using GalleriaService.Api.Features.TagCloud;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Galleria API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Galleria API v1");
        c.RoutePrefix = string.Empty; // Serve the Swagger UI at the root
    });
}

app.UseHttpsRedirection();

app.MapGroup("/api/file").WithFileManagementResourcesV1().WithTags("File Management");
app.MapGroup("/api/gallery").WithGalleryResourcesV1().WithTags("Gallery Management");
app.MapGroup("/api/tagcloud").WithTagCloudResourcesV1().WithTags("Tag Cloud Management");

// app.MapFileManagementRoutes();
// app.MapGalleryRoutes();
// app.MapTagCloudRoutes();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
