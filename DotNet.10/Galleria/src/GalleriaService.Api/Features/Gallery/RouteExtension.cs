using GalleriaService.Api.Features;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace GalleriaService.Api.Features.Gallery;

public static class RouteExtension
{
    public static RouteGroupBuilder MapGalleryRoutes(this RouteGroupBuilder group)
    {
        group.MapGet("/all", GetAllGalleries);
        group.MapGet("/{galleryId}", GetByGalleryId);
        return group;
    }

    internal static async Task<IResult> GetAllGalleries()
    {
        //Services.ExecuteGetAllGalleriesAsync().GetAwaiter().GetResult();
        return await Task.FromResult(Results.Ok("All galleries returned.")); 
    }

    internal static async Task<IResult> GetByGalleryId(string galleryId)
    {
        //Services.ExecuteGetByGalleryIdAsync(galleryId).GetAwaiter().GetResult();
        return await Task.FromResult(Results.Ok($"Gallery for id: {galleryId}"));
    }
}
