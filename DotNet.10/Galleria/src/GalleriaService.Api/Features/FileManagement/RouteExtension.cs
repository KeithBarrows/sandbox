using GalleriaService.Api.Features;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace GalleriaService.Api.Features.FileManagement;

public static class RouteExtension
{
    public static RouteGroupBuilder WithFileManagementResourcesV1(this RouteGroupBuilder group)
    {
        group.MapPost("/create", GenerateFileInfo);
        group.MapPost("/duplicates", IdentifyDuplicates);
        group.MapGet("/folder/{folderId}", GetByFolder);
        group.MapGet("/tag/{tagId}", GetByTag);
        return group;
    }

    internal static async Task<IResult> GenerateFileInfo()
    {
        await Services.ExecuteFileInfoGenerationAsync();
        return Results.Ok("File info generated successfully.");
    }

    internal static async Task<IResult> IdentifyDuplicates()
    {
        await Services.ExecuteDuplicateIdentificationAsync();
        return Results.Ok("Duplicate files identified successfully.");
    }

    internal static async Task<IResult> GetByFolder()
    {
        //Services.ExecuteGetByFolderAsync().GetAwaiter().GetResult();
        return await Task.FromResult(Results.Ok("Files for the specified folder."));
    }

    internal static async Task<IResult> GetByTag()
    {
        //Services.ExecuteGetByTagAsync().GetAwaiter().GetResult();
        return await Task.FromResult(Results.Ok("Files for the specified tag."));
    }
}
