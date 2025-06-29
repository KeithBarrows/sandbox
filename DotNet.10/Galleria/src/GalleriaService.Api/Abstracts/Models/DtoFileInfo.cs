using System.Text.Json.Serialization;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GalleriaService.Api.Abstracts.Models;

public class DtoFileInfo : INotifyPropertyChanged
{
    private string _path = string.Empty;
    private string _fileName = string.Empty;
    private string _fileType = string.Empty;
    private List<DtoFileInfoTag> _tags = new();
    private List<string> _duplicates = [];
    private bool _toBeDeleted = false;
    private bool _isChecked = false;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    [JsonPropertyName("path")]
    public string Path 
    { 
        get => _path;
        set => SetField(ref _path, value);
    }

    [JsonPropertyName("fullPath")]
    public string FullPath => GenerateFullPath();

    [JsonPropertyName("fileName")]
    public string FileName 
    { 
        get => _fileName;
        set => SetField(ref _fileName, value);
    }

    [JsonPropertyName("fileType")]
    public string FileType 
    { 
        get => _fileType;
        set => SetField(ref _fileType, value);
    }

    [JsonPropertyName("tags")]
    public List<DtoFileInfoTag> Tags 
    { 
        get => _tags;
        set
        {
            if (SetField(ref _tags, value))
            {
                OnPropertyChanged(nameof(Breadcrumbs));
            }
        }
    }

    [JsonPropertyName("breadcrumbs")]
    public List<string> Breadcrumbs => GenerateBreadcrumbs();

    [JsonPropertyName("duplicates")]
    public List<string> Duplicates 
    { 
        get => _duplicates;
        set => SetField(ref _duplicates, value);
    }

    [JsonPropertyName("toBeDeleted")]
    public bool ToBeDeleted 
    { 
        get => _toBeDeleted;
        set => SetField(ref _toBeDeleted, value);
    }

    [JsonPropertyName("isChecked")]
    public bool IsChecked 
    { 
        get => _isChecked;
        set => SetField(ref _isChecked, value);
    }

    private string GenerateFullPath()
    {
        var rootPrefix = Path.Substring(0, 2);
        var rootFolder = Models.Tags.PrefixMap.FirstOrDefault(a => a.Key == rootPrefix).Value;
        var fullPath = System.IO.Path.Combine(@"I:\", rootFolder, Path);
        return fullPath;
    }

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

public class DtoFileInfoTag : INotifyPropertyChanged
{
    private string _type = string.Empty;
    private string _value = string.Empty;
    private int _imageCount = 0;
    private int _videoCount = 0;
    private int _bucket = 0;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    [JsonPropertyName("type")]
    public string Type 
    { 
        get => _type;
        set => SetField(ref _type, value);
    }

    [JsonPropertyName("value")]
    public string Value 
    { 
        get => _value;
        set => SetField(ref _value, value);
    }

    // [JsonPropertyName("tagWeight")]
    // public int TagWeight 
    // {
    //     get
    //     {
    //         var result = ImageCount + (VideoCount * 25);
    //         OnPropertyChanged(nameof(TagWeight));
    //         return result;
    //     }
    // }

    [JsonPropertyName("imageCount")]
    public int ImageCount 
    { 
        get => _imageCount;
        set => SetField(ref _imageCount, value);
    }

    [JsonPropertyName("videoCount")]
    public int VideoCount 
    { 
        get => _videoCount;
        set => SetField(ref _videoCount, value);
    }

    [JsonPropertyName("bucket")]
    public int Bucket 
    { 
        get => _bucket;
        set => SetField(ref _bucket, value);
    }

    public DtoFileInfoTag() { }

    public DtoFileInfoTag(string type, string value)
    {
        Type = type;
        Value = value;
    }
}