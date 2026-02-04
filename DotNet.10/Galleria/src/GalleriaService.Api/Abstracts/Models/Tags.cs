
namespace GalleriaService.Api.Abstracts.Models;

public static class Tags
{
    public const string Person = "p-";
    public const string Album = "a-";
    public const string Studio = "s-";
    public const string Type = "t-";
    public const string Gallery = "g-";
    public const string Category = "c-";
    public const string Highlight = "h-";


    public static Dictionary<string, string> PrefixMap => new()
    {
        { "p-", "Person" },
        { "a-", "Album" },
        { "s-", "Studio" },
        { "t-", "Type" },
        { "g-", "Gallery" },
        { "c-", "Category" },
        { "h-", "Highlight" }
    };
    public static Dictionary<string, string> PostfixMap => new()
    {
        { "Person", "p-" },
        { "Album", "a-" },
        { "Studio", "s-" },
        { "Type", "t-" },
        { "Gallery", "g-" },
        { "Category", "c-" },
        { "Highlight", "h-" }
    };
}
