using System.Text;
using System.Text.Json;
using GalleriaService.Api.Abstracts.Interfaces;
using GalleriaService.Api.Abstracts.Models;

namespace GalleriaService.Api.Features.FileManagement.GenerateTags;

public class Service : IService
{
    private readonly string[] _rootFolders = new[]
    {
        @"I:\Album",
        @"I:\Category",
        @"I:\Highlight",
        @"I:\Person",
        @"I:\Studio"
    };

    private readonly HashSet<string> _allowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        // Images
        ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".webp", ".heic", ".raw", ".cr2", ".nef",
        // Videos
        ".mp4", ".mov", ".avi", ".wmv", ".mkv", ".m4v", ".webm", ".flv", ".mpeg", ".mpg"
    };

    public Service() { }

    public async Task ExecuteAsync()
    {
        // Delete existing master file if it exists
        var masterFilePath = @"I:\.files.metadata.json";
        if (File.Exists(masterFilePath))
        {
            File.Delete(masterFilePath);
        }

        // Process all root folders
        foreach (var rootFolder in _rootFolders)
        {
            if (Directory.Exists(rootFolder))
            {
                await GenerateTagsAsync(rootFolder);
            }
        }

        // Gather and combine all metadata files
        var allFiles = new List<DtoFileInfo>();
        foreach (var rootFolder in _rootFolders)
        {
            if (!Directory.Exists(rootFolder)) continue;

            // Find all metadata files in this root folder and its subfolders
            var metadataFiles = Directory.GetFiles(rootFolder, ".files.metadata.json", SearchOption.AllDirectories);
            foreach (var metadataFile in metadataFiles)
            {
                var content = await File.ReadAllTextAsync(metadataFile);
                var files = JsonSerializer.Deserialize<List<DtoFileInfo>>(content);
                if (files != null)
                {
                    allFiles.AddRange(files);
                }
            }
        }

        // Write the combined data to the master file
        if (allFiles.Any())
        {
            var jsonContent = JsonSerializer.Serialize(allFiles, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            await File.WriteAllTextAsync(masterFilePath, jsonContent);
        }
    }

    private async Task GenerateTagsAsync(string rootPath)
    {
        if (!_rootFolders.Contains(rootPath))
        {
            throw new ArgumentException($"Invalid root path: {rootPath}. Must be one of the configured root folders.");
        }

        var directChildFolders = Directory.GetDirectories(rootPath);

        foreach (var childFolder in directChildFolders)
        {
            await ProcessFolderAsync(childFolder, rootPath);
        }
    }

    private async Task ProcessFolderAsync(string folderPath, string rootPath)
    {
        var files = new List<DtoFileInfo>();
        await ProcessFilesInFolderAsync(folderPath, rootPath, files);

        if (files.Any())
        {
            var jsonFilePath = Path.Combine(folderPath, ".files.metadata.json");
            var jsonContent = JsonSerializer.Serialize(files, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            await File.WriteAllTextAsync(jsonFilePath, jsonContent);
        }
    }

    private async Task ProcessFilesInFolderAsync(string currentFolder, string rootPath, List<DtoFileInfo> files)
    {
        // Process files in current folder
        foreach (var file in Directory.GetFiles(currentFolder))
        {
            var extension = Path.GetExtension(file);
            if (!_allowedExtensions.Contains(extension)) continue;

            var fileInfo = new DtoFileInfo
            {
                Path = GetRelativePath(file, rootPath),
                FileName = Path.GetFileName(file),
                FileType = extension,
                Tags = GetTagsFromPath(file, rootPath)
            };
            files.Add(fileInfo);
        }

        // Process subfolders recursively
        foreach (var subFolder in Directory.GetDirectories(currentFolder))
        {
            await ProcessFilesInFolderAsync(subFolder, rootPath, files);
        }
    }

    private string GetRelativePath(string fullPath, string rootPath)
    {
        return Path.GetRelativePath(rootPath, fullPath).Replace('\\', '/');
    }

    private List<DtoFileInfoTag> GetTagsFromPath(string filePath, string rootPath)
    {
        var tags = new List<DtoFileInfoTag>();
        var relativePath = GetRelativePath(filePath, rootPath);
        var pathSegments = relativePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

        foreach (var segment in pathSegments)
        {
            foreach (var prefix in Tags.PrefixMap)
            {
                if (segment.StartsWith(prefix.Key, StringComparison.OrdinalIgnoreCase))
                {
                    tags.Add(new DtoFileInfoTag(prefix.Value, segment[2..])); // Skip the prefix length
                    break;
                }
            }
        }

        return tags;
    }
}