
namespace GalleriaService.Api.Abstracts.Models;

public static class RootFolders
{
    public const string Person = "Person";
    public const string Album = "Album";
    public const string Studio = "Studio";
    public const string Category = "Category";
    public const string Highlight = "Highlight";

    public static Dictionary<string, string> FolderMap => new()
    {
        { Person, @"I:\Person" },
        { Album, @"I:\Album" },
        { Studio, @"I:\Studio" },
        { Category, @"I:\Category" },
        { Highlight, @"I:\Highlight" }
    };
}

public static class AllowedImageTypes
{
    public static HashSet<string> Extensions => _allowedExtensions; 

    private static readonly HashSet<string> _allowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        // Images
        ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".webp", ".heic", ".raw", ".cr2", ".nef",
    };
}

public static class AllowedVideoTypes
{
    public static HashSet<string> Extensions => _allowedExtensions;

    private static readonly HashSet<string> _allowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        // Videos
        ".mp4", ".mov", ".avi", ".wmv", ".mkv", ".m4v", ".webm", ".flv", ".mpeg", ".mpg", "*vid"
    };
}