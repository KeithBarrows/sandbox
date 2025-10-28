using GalleriaService.Api.Features;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace GalleriaService.Api.Features.TagCloud;

public static class RouteExtension
{
    public static RouteGroupBuilder MapTagCloudRoutes(this RouteGroupBuilder group)
    {
        group.MapGet("/all", GetAllTags);
        group.MapGet("/{tag}", GetByTag);
        group.MapPost("/", CreateTagCloud);
        return group;
    }

    internal static async Task<IResult> GetAllTags()
    {
        await Services.ExecuteTagCloudCreationAsync();
        return Results.Ok("All tags returned.");
    }

    internal static async Task<IResult> GetByTag(string tag)
    {
        await Services.ExecuteTagCloudCreationAsync(); //.GetAwaiter().GetResult();
        return Results.Ok($"Files for tag: {tag}");
    }

    internal static async Task<IResult> CreateTagCloud()
    {
        await Services.ExecuteTagCloudCreationAsync();
        return Results.Ok("Tag cloud created successfully.");
    }
}
