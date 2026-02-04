using System.Text.Json;
using GalleriaService.Api.Abstracts.Interfaces;
using GalleriaService.Api.Abstracts.Models;
using GalleriaService.Api.Abstracts.Utilities;

namespace GalleriaService.Api.Features.TagCloud.CreateTagCloud;

public class CreateTagCloud : IService
{
    private const string FilesPath = @"I:\.files.metadata.json";
    private const string TagsPath = @"I:\.tag.cloud.json";
    private const int NumberOfBuckets = 5;

    public async Task ExecuteAsync()
    {
        // Read the metadata file
        var json = await File.ReadAllTextAsync(FilesPath);
        var files = JsonSerializer.Deserialize<List<DtoFileInfo>>(json) ?? [];
        var tagCloud = new List<DtoTagInfo>();

        // Process each tag type
        foreach (var tagPrefix in Tags.PrefixMap.Keys)
        {
            List<DtoTagInfo> tagCounts = [];

            // Process each file
            foreach (var file in files.Where(f => f.Tags.Any(t => t.Type == tagPrefix) && !f.ToBeDeleted))
            {
                var isImage = AllowedImageTypes.Extensions.Contains(file.FileType);
                var isVideo = AllowedVideoTypes.Extensions.Contains(file.FileType);

                // Process tags of current type
                foreach (var tag in file.Tags.Where(t => t.Type == tagPrefix))
                {
                    var existingTag = tagCounts.FirstOrDefault(t => t.Tag == tag.Value);
                    if (existingTag == null)
                    {
                        existingTag = new DtoTagInfo
                        {
                            Type = tag.Type,
                            Tag = tag.Value,
                            Count = 0
                        };
                        tagCounts.Add(existingTag);
                    }

                    if (isImage) tag.ImageCount++;
                    if (isVideo) tag.VideoCount++;
                    existingTag.Count = existingTag.Count + tag.ImageCount + (tag.VideoCount * 25);
                }
            }

            // Assign buckets to tags
            var tagList = TagBucketing.AssignBuckets(tagCounts, NumberOfBuckets);
            tagCloud.AddRange(tagList);

            // // Update the buckets in the original file data
            // foreach (var file in files)
            // {
            //     foreach (var tag in file.Tags.Where(t => t.Type == tagPrefix))
            //     {
            //         tag.Bucket = tagList.First(t => t.Tag == tag.Value).Bucket;
            //     }
            // }
        }

        // Write the updated data back to the file
        var options = new JsonSerializerOptions { WriteIndented = true };
        await File.WriteAllTextAsync(TagsPath, JsonSerializer.Serialize(tagCloud, options));
    }
}