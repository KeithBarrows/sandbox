using System.Text.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using GalleriaService.Api.Abstracts.Models;
using FFMpegCore;
using FFMpegCore.Arguments;
using System.Security.Cryptography;

namespace GalleriaService.Api.Features.FileManagement.IdentifyDuplicates;

public class DuplicateIdentifier
{
    private const int MinImageDimension = 128;
    private const int VideoComparisonStartSeconds = 120; // 2 minutes in
    private const int VideoComparisonDurationSeconds = 60;
    private const int SaveDebounceMilliseconds = 1000; // Wait 1 second after last change before saving
    private readonly string _metadataFilePath;
    private readonly Dictionary<string, List<string>> _duplicateGroups;
    private List<DtoFileInfo> _metadata = [];
    private readonly SemaphoreSlim _saveSemaphore = new(1, 1);
    private CancellationTokenSource _saveCancellation = new();
    private Task? _saveTask;

    public DuplicateIdentifier(string metadataFilePath)
    {
        _metadataFilePath = metadataFilePath;
        _duplicateGroups = new Dictionary<string, List<string>>();
    }

    public async Task ProcessAllAsync()
    {        var jsonContent = await File.ReadAllTextAsync(_metadataFilePath);
        _metadata = JsonSerializer.Deserialize<List<DtoFileInfo>>(jsonContent) ?? [];

        // Attach event handler to save metadata on any field change
        foreach (var item in _metadata)
        {
            item.PropertyChanged += (sender, args) =>
            {
                _saveTask = SaveMetadataAsync();
            };
        }

        // First process images
        await ProcessImagesAsync();

        // // Then process videos
        // var vidList = _metadata.Where(m => IsVideo(m.FullPath)).ToList();
        // await ProcessVideosAsync(vidList);

        // Update the metadata file with duplicate information
        await UpdateMetadataWithDuplicates(_metadata);
    }

    private async Task ProcessImagesAsync()
    {
        var index = new IdentifyDuplicatesIndex();
        await index.ReadFromDisc();

        var duplicatesModel = new IdentifyDuplicatesModel();
        await duplicatesModel.ReadFromDisc();

        var tagList = _metadata.SelectMany(m => m.Tags).Select(t => t.Value).Distinct().ToList();
        var tagsRun = index.Entries.Select(e => e.Tag).Distinct().ToList();
        foreach (var tag in tagList)
        {
            var entries = index.Entries.Where(e => e.Tag == tag).ToList();
            var isCompleted = index.IsCompleted.ContainsKey(tag) && index.IsCompleted[tag];
            var outerIndex = 0;
            var innerIndex = 1;
            if (isCompleted)
            {
                Console.WriteLine($"Skipping completed tag: {tag}");
                continue;
            }
            if(index.IsCompleted.ContainsKey(tag))
            {
                outerIndex = index.LastOuterIndex;
                innerIndex = index.LastInnerIndex;
                Console.WriteLine($"Continuing tag: {tag} at {outerIndex} | {innerIndex }");
            }
            else
                Console.WriteLine($"Starting Tag = {tag}");

            var imageFiles = _metadata.Where(m => AllowedImageTypes.Extensions.Contains(m.FileType)
                                               && m.ToBeDeleted == false
                                               && m.Tags.Any(t => t.Value == tag))
                                      .ToList();

            // get the last record in the List...
            for (int i = outerIndex; i < imageFiles.Count; i++)
            {
                //if(imageFiles[i].IsChecked) continue;

                var file1 = imageFiles[i];
                var entry = new Entry(tag, i, innerIndex, file1.FullPath, string.Empty);
                await index.AddOrUpdate(entry);
                if (imageFiles.Count > 1)
                {
                    for (int j = innerIndex; j < imageFiles.Count; j++)
                    {
                        var file2 = imageFiles[j];
                        entry.FileInner = file2.FullPath;
                        entry.IndexInner = j;
                        await index.AddOrUpdate(entry);

                        if (await AreImagesVisuallyIdenticalAsync(file1.FullPath, file2.FullPath))
                        {
                            duplicatesModel.AddOrUpdate(tag, file1.FullPath, file2.FullPath);
                            Console.WriteLine($"{i} | {j} - {file1.FullPath} == {file2.FullPath}");
                        }
                        else
                        {
                            Console.WriteLine($"** {i} | {j} **");
                        }
                    }
                }
                imageFiles[i].IsChecked = true;
                if (duplicatesModel.TagPathJoins.Count > 0)
                    await duplicatesModel.WriteToDisc();
            }
            index.CompleteTag(tag);
            Console.WriteLine($"Completed Tag = {tag}");
        }
    }

    private async Task ProcessVideosAsync(List<DtoFileInfo> videoFiles)
    {
        for (int i = 0; i < videoFiles.Count; i++)
        {
            var file1 = videoFiles[i];
            if (!IsInAllowedRootFolder(file1.FullPath)) continue;

            var tasks = new List<Task>();
            for (int j = i + 1; j < videoFiles.Count; j++)
            {
                var file2 = videoFiles[j];
                if (!IsInAllowedRootFolder(file2.FullPath)) continue;

                tasks.Add(Task.Run(async () =>
                {
                    if (await AreVideosVisuallyIdenticalAsync(file1.FullPath, file2.FullPath))
                    {
                        lock (_duplicateGroups)
                        {
                            AddToDuplicateGroup(file1.FullPath, file2.FullPath);
                            Console.WriteLine($"{i} | {j} -  - {file1.FullPath} != {file2.FullPath}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"** {i} | {j} **");
                    }
                }));
            }
            await Task.WhenAll(tasks);
        }
    }

    private async Task<bool> AreImagesVisuallyIdenticalAsync(string path1, string path2)
    {
        Image? image1 = null;
        Image? image2 = null;
        try
        {
            // Try to load image1, attempt repair if needed
            try
            {
                image1 = await Image.LoadAsync(path1);
            }
            catch
            {
                var repairedPath = Path.GetTempFileName();
                try
                {
                    using (var img = Image.Load(path1))
                    {
                        img.Save(repairedPath, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder());
                    }
                    File.Delete(path1);
                    File.Move(repairedPath, path1);
                    image1 = await Image.LoadAsync(path1);
                    Console.WriteLine($"Image repaired and replaced: {path1}");
                }
                catch
                {
                    var rec = _metadata.FirstOrDefault(m => m.FullPath == path1);
                    if (rec != null) rec.ToBeDeleted = true;
                    if (File.Exists(repairedPath)) File.Delete(repairedPath);
                    return false;
                }
            }

            // Try to load image2, attempt repair if needed
            try
            {
                image2 = await Image.LoadAsync(path2);
            }
            catch
            {
                var repairedPath = Path.GetTempFileName();
                try
                {
                    using (var img = Image.Load(path2))
                    {
                        img.Save(repairedPath, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder());
                    }
                    File.Delete(path2);
                    File.Move(repairedPath, path2);
                    image2 = await Image.LoadAsync(path2);
                    Console.WriteLine($"Image repaired and replaced: {path2}");
                }
                catch
                {
                    var rec = _metadata.FirstOrDefault(m => m.FullPath == path2);
                    if (rec != null) rec.ToBeDeleted = true;
                    if (File.Exists(repairedPath)) File.Delete(repairedPath);
                    return false;
                }
            }

            // Resize images to comparable dimensions while maintaining aspect ratio
            var resized1 = ResizeForComparison(image1);
            var resized2 = ResizeForComparison(image2);

            // Compare pixel data
            return await CompareImagesAsync(resized1, resized2);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error comparing images: {ex.Message}");
            return false;
        }
        finally
        {
            image1?.Dispose();
            image2?.Dispose();
        }
    }

    private async Task<bool> AreVideosVisuallyIdenticalAsync(string path1, string path2)
    {
        try
        {
            var snapshot1 = await ExtractVideoSnapshotAsync(path1);
            var snapshot2 = await ExtractVideoSnapshotAsync(path2);

            if (snapshot1 == null || snapshot2 == null)
                return false;

            // Compare the snapshots
            return await CompareImagesAsync(snapshot1, snapshot2);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error comparing videos: {ex.Message}");
            return false;
        }
    }

    private Image ResizeForComparison(Image image)
    {        // Calculate new dimensions maintaining aspect ratio
        var ratio = Math.Max((double)MinImageDimension / image.Width, (double)MinImageDimension / image.Height);
        var newWidth = (int)(image.Width * ratio);
        var newHeight = (int)(image.Height * ratio);

        var clone = image.Clone(x => x.Resize(newWidth, newHeight));
        return clone;
    }

    private async Task<Image?> ExtractVideoSnapshotAsync(string videoPath)
    {
        try
        {
            var outputPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.png"); await FFMpegArguments
                .FromFileInput(videoPath)
                .OutputToFile(outputPath, true, options => options
                    .WithCustomArgument($"-ss {VideoComparisonStartSeconds}")
                    .WithCustomArgument("-vframes 1")
                    .WithCustomArgument("-f image2"))
                .ProcessAsynchronously();

            var snapshot = await Image.LoadAsync(outputPath);
            File.Delete(outputPath); // Clean up temp file

            return snapshot;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error extracting video snapshot: {ex.Message}");
            return null;
        }
    }

    private async Task<bool> CompareImagesAsync(Image image1, Image image2)
    {
        if (image1.Width != image2.Width || image1.Height != image2.Height)
            return false;

        using var ms1 = new MemoryStream();
        using var ms2 = new MemoryStream();

        await image1.SaveAsPngAsync(ms1);
        await image2.SaveAsPngAsync(ms2);

        var hash1 = ComputeImageHash(ms1.ToArray());
        var hash2 = ComputeImageHash(ms2.ToArray());

        return hash1.SequenceEqual(hash2);
    }

    private byte[] ComputeImageHash(byte[] imageData)
    {
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(imageData);
    }

    private void AddToDuplicateGroup(string path1, string path2)
    {
        var groupId = GetGroupIdForPath(path1) ?? CreateNewGroup(path1);
        AddPathToGroup(groupId, path2);
    }

    private string? GetGroupIdForPath(string path)
    {
        return _duplicateGroups.FirstOrDefault(g => g.Value.Contains(path)).Key;
    }

    private string CreateNewGroup(string initialPath)
    {
        var groupId = Guid.NewGuid().ToString();
        _duplicateGroups[groupId] = new List<string> { initialPath };
        return groupId;
    }

    private void AddPathToGroup(string groupId, string path)
    {
        if (!_duplicateGroups[groupId].Contains(path))
        {
            _duplicateGroups[groupId].Add(path);
        }
    }

    private bool IsInAllowedRootFolder(string path)
    {
        return RootFolders.FolderMap.Values.Any(rootPath => 
            path.StartsWith(rootPath, StringComparison.OrdinalIgnoreCase));
    }

    private bool IsImage(string path)
    {
        return AllowedImageTypes.Extensions.Contains(Path.GetExtension(path));
    }

    private bool IsVideo(string path)
    {
        return AllowedVideoTypes.Extensions.Contains(Path.GetExtension(path));
    }

    private async Task UpdateMetadataWithDuplicates(List<DtoFileInfo> metadata)
    {
        foreach (var group in _duplicateGroups)
        {
            foreach (var path in group.Value)
            {
                var fileMetadata = metadata.Find(m => m.FullPath == path);
                if (fileMetadata != null)
                {
                    if (fileMetadata.Duplicates == null)
                        fileMetadata.Duplicates = new List<string>();
                    
                    fileMetadata.Duplicates.AddRange(
                        group.Value.Where(p => p != path && !fileMetadata.Duplicates.Contains(p))
                    );
                }
            }
        }

        var options = new JsonSerializerOptions { WriteIndented = true };
        await File.WriteAllTextAsync(_metadataFilePath, 
            JsonSerializer.Serialize(metadata, options));
    }

    private async Task SaveMetadataAsync()
    {
        try
        {
            await _saveSemaphore.WaitAsync();
            
            // Cancel any pending save operation
            _saveCancellation.Cancel();
            _saveCancellation = new CancellationTokenSource();
            var token = _saveCancellation.Token;

            // Wait for the debounce period
            try
            {
                await Task.Delay(SaveDebounceMilliseconds, token);
            }
            catch (TaskCanceledException)
            {
                // Another save was requested, exit this one
                return;
            }

            // Perform the save
            var options = new JsonSerializerOptions { WriteIndented = true };
            await using var fileStream = new FileStream(_metadataFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await using var writer = new StreamWriter(fileStream);
            await writer.WriteAsync(JsonSerializer.Serialize(_metadata, options));
        }
        finally
        {
            _saveSemaphore.Release();
        }
    }
}

