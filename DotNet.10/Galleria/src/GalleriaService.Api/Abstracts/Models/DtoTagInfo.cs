using System.Text.Json.Serialization;

namespace GalleriaService.Api.Abstracts.Models;

public class DtoTagInfo
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("tag")]
    public string Tag { get; set; } = string.Empty;

    [JsonPropertyName("bucket")]
    public int Bucket { get; set; } = 0;

    [JsonPropertyName("count")]
    public int Count { get; set; } = 0;
}
