using System.Text.Json.Serialization;

namespace GalleriaService.Api.Features.FileManagement.GenerateTags;

public class DtoFileInfo
{
    [JsonPropertyName("path")]
    public string Path { get; set; } = string.Empty;

    [JsonPropertyName("fileName")]
    public string FileName { get; set; } = string.Empty;

    [JsonPropertyName("fileType")]
    public string FileType { get; set; } = string.Empty;

    [JsonPropertyName("tags")]
    public List<DtoFileInfoTag> Tags { get; set; } = new();

    [JsonPropertyName("breadcrumbs")]
    public List<string> Breadcrumbs => GenerateBreadcrumbs();

    private List<string> GenerateBreadcrumbs()
    {
        var breadcrumbs = new List<string>();
        var personTags = Tags.Where(t => t.Type == "p-").OrderBy(t => t.Value).ToList();
        var nonPersonTags = Tags.Where(t => t.Type != "p-")
                               .GroupBy(t => t.Type)
                               .OrderBy(g => GetTagTypeOrder(g.Key))
                               .ToDictionary(g => g.Key, g => g.OrderBy(t => t.Value).Select(t => t.Value));

        // If no person tags, create a single breadcrumb with all other tags
        if (!personTags.Any())
        {
            var breadcrumb = string.Join(" | ", nonPersonTags.SelectMany(g => g.Value));
            breadcrumbs.Add(breadcrumb);
            return breadcrumbs;
        }

        // Create a breadcrumb for each person
        foreach (var personTag in personTags)
        {
            var parts = new List<string> { personTag.Value };
            parts.AddRange(nonPersonTags.SelectMany(g => g.Value));
            breadcrumbs.Add(string.Join(" | ", parts));
        }

        return breadcrumbs;
    }

    private int GetTagTypeOrder(string tagType) => tagType switch
    {
        "p-" => 1,
        "a-" => 2,
        "s-" => 3,
        "t-" => 4,
        "g-" => 5,
        "c-" => 6,
        "h-" => 7,
        _ => 999
    };
}

public class DtoFileInfoTag
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;

    public DtoFileInfoTag() { }

    public DtoFileInfoTag(string type, string value)
    {
        Type = type;
        Value = value;
    }
}