using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ImageScraper
{
    public class DirectoryOrganizer
    {
        private const string STUDIO_BASE_PATH = @"I:\Studio";
        private const string CATEGORIES_BASE_PATH = @"I:\Category";
        private const string HIGHLIGHTS_BASE_PATH = @"I:\Highlight";
        private const string ALBUMS_BASE_PATH = @"I:\Album";
        
        /// <summary>
        /// Organizes gallery information into the specified directory structure
        /// </summary>
        /// <param name="galleryInfo">The parsed gallery information</param>
        /// <returns>The final directory path where images should be saved</returns>
        public static string OrganizeGalleryDirectories(HtmlGalleryParser.GalleryInfo galleryInfo)
        {
            try
            {
                Console.WriteLine("Organizing directory structure...");
                
                // Step 1: Find or create studio directory
                string studioPath = FindOrCreateStudioDirectory(galleryInfo.Channel);
                if (string.IsNullOrEmpty(studioPath))
                {
                    Console.WriteLine($"Could not find or create studio directory for channel: {galleryInfo.Channel}");
                    return string.Empty;
                }
                
                // Step 2: Find or create gallery directory
                string galleryPath = FindOrCreateGalleryDirectory(studioPath, galleryInfo.Channel);
                if (string.IsNullOrEmpty(galleryPath))
                {
                    Console.WriteLine($"Could not find or create gallery directory for channel: {galleryInfo.Channel}");
                    return string.Empty;
                }
                
                // Step 3: Create person directories for each model
                string finalPath = galleryPath;
                foreach (var model in galleryInfo.Models)
                {
                    finalPath = CreatePersonDirectory(finalPath, model);
                }
                
                // Step 4: Apply categories and tags to multiple root directories
                finalPath = ApplyCategoriesToDirectories(finalPath, galleryInfo.Categories, galleryInfo.TagsList);
                
                // Step 5: Create title directory as the final directory
                if (!string.IsNullOrEmpty(galleryInfo.Title))
                {
                    finalPath = CreateTitleDirectory(finalPath, galleryInfo.Title);
                }
                
                Console.WriteLine($"Final directory path: {finalPath}");
                return finalPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error organizing directories: {ex.Message}");
                return string.Empty;
            }
        }
        
        /// <summary>
        /// Finds or creates a studio directory based on the channel name
        /// </summary>
        private static string FindOrCreateStudioDirectory(string channelName)
        {
            if (string.IsNullOrEmpty(channelName))
                return string.Empty;
                
            if (!Directory.Exists(STUDIO_BASE_PATH))
            {
                Console.WriteLine($"Studio base path does not exist: {STUDIO_BASE_PATH}");
                return string.Empty;
            }
            
            // Get all studio directories (starting with 's-')
            var studioDirectories = Directory.GetDirectories(STUDIO_BASE_PATH, "s-*");
            
            foreach (var studioDir in studioDirectories)
            {
                var dirName = Path.GetFileName(studioDir);
                // Remove 's-' prefix for comparison
                var studioName = dirName.Substring(2);
                
                // Check for exact match or partial match
                if (studioName.Equals(channelName, StringComparison.OrdinalIgnoreCase) ||
                    studioName.Contains(channelName, StringComparison.OrdinalIgnoreCase) ||
                    channelName.Contains(studioName, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Found matching studio directory: {studioDir}");
                    return studioDir;
                }
            }
            
            // If no match found, create new studio directory
            var newStudioPath = Path.Combine(STUDIO_BASE_PATH, $"s-{channelName}");
            try
            {
                Directory.CreateDirectory(newStudioPath);
                Console.WriteLine($"Created new studio directory: {newStudioPath}");
                return newStudioPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create studio directory {newStudioPath}: {ex.Message}");
                return string.Empty;
            }
        }
        
        /// <summary>
        /// Finds or creates a gallery directory within the studio directory
        /// </summary>
        private static string FindOrCreateGalleryDirectory(string studioPath, string channelName)
        {
            if (string.IsNullOrEmpty(studioPath) || string.IsNullOrEmpty(channelName))
                return string.Empty;
                
            // Get all gallery directories (starting with 'g-')
            var galleryDirectories = Directory.GetDirectories(studioPath, "g-*");
            
            foreach (var galleryDir in galleryDirectories)
            {
                var dirName = Path.GetFileName(galleryDir);
                // Remove 'g-' prefix for comparison
                var galleryName = dirName.Substring(2);
                
                // Check for exact match
                if (galleryName.Equals(channelName, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Found matching gallery directory: {galleryDir}");
                    return galleryDir;
                }
            }
            
            // If no match found, create new gallery directory
            var newGalleryPath = Path.Combine(studioPath, $"g-{channelName}");
            try
            {
                Directory.CreateDirectory(newGalleryPath);
                Console.WriteLine($"Created new gallery directory: {newGalleryPath}");
                return newGalleryPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create gallery directory {newGalleryPath}: {ex.Message}");
                return string.Empty;
            }
        }
        
        /// <summary>
        /// Creates a person directory for a model
        /// </summary>
        private static string CreatePersonDirectory(string parentPath, string modelName)
        {
            if (string.IsNullOrEmpty(parentPath) || string.IsNullOrEmpty(modelName))
                return parentPath;
                
            var personPath = Path.Combine(parentPath, $"p-{modelName}");
            try
            {
                Directory.CreateDirectory(personPath);
                Console.WriteLine($"Created person directory: {personPath}");
                return personPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create person directory {personPath}: {ex.Message}");
                return parentPath;
            }
        }
        
        /// <summary>
        /// Applies categories and tags to multiple root directories (Album, Highlight, Category)
        /// </summary>
        private static string ApplyCategoriesToDirectories(string parentPath, List<string> categories, List<string> tagsList)
        {
            if (string.IsNullOrEmpty(parentPath))
                return parentPath;

            var allItems = new List<string>();
            if (categories != null) allItems.AddRange(categories);
            if (tagsList != null) allItems.AddRange(tagsList);

            if (!allItems.Any())
                return parentPath;

            // Define allowed highlights
            var allowedHighlights = new[] { "Vintage", "Glamour", "Retro", "Classic", "Russian", "Mature", "Outdoor", "BlackAndWhite", "Tall" };

            string currentPath = parentPath;

            // Process Album directories (a-)
            currentPath = ProcessDirectoryType(currentPath, allItems, ALBUMS_BASE_PATH, "a-", "Album");

            // Process Highlight directories (h-)
            currentPath = ProcessHighlightDirectories(currentPath, allItems, allowedHighlights);

            // Process Category directories (c-)
            currentPath = ProcessDirectoryType(currentPath, allItems, CATEGORIES_BASE_PATH, "c-", "Category");

            return currentPath;
        }

        /// <summary>
        /// Processes a specific directory type (Album or Category)
        /// </summary>
        private static string ProcessDirectoryType(string parentPath, List<string> items, string basePath, string prefix, string typeName)
        {
            if (string.IsNullOrEmpty(parentPath) || !Directory.Exists(basePath))
            {
                Console.WriteLine($"{typeName} base path does not exist: {basePath}");
                return parentPath;
            }

            try
            {
                var directories = Directory.GetDirectories(basePath, $"{prefix}*");
                var directoryNames = directories.Select(dir => 
                {
                    var dirName = Path.GetFileName(dir);
                    return dirName.Substring(prefix.Length); // Remove prefix
                }).ToList();

                string currentPath = parentPath;

                foreach (var item in items)
                {
                    var matchingDirectory = directoryNames.FirstOrDefault(dirName => 
                        dirName.Equals(item, StringComparison.OrdinalIgnoreCase));

                    if (!string.IsNullOrEmpty(matchingDirectory))
                    {
                        var itemPath = Path.Combine(currentPath, $"{prefix}{matchingDirectory}");
                        try
                        {
                            Directory.CreateDirectory(itemPath);
                            Console.WriteLine($"Created {typeName.ToLower()} directory: {itemPath}");
                            currentPath = itemPath;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to create {typeName.ToLower()} directory {itemPath}: {ex.Message}");
                        }
                    }
                }

                return currentPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing {typeName.ToLower()} directories: {ex.Message}");
                return parentPath;
            }
        }

        /// <summary>
        /// Processes highlight directories with special logic for allowed highlights
        /// </summary>
        private static string ProcessHighlightDirectories(string parentPath, List<string> items, string[] allowedHighlights)
        {
            if (string.IsNullOrEmpty(parentPath))
                return parentPath;

            // Get highlights from the Highlight directory if it exists
            var availableHighlights = new List<string>(allowedHighlights);

            if (Directory.Exists(HIGHLIGHTS_BASE_PATH))
            {
                try
                {
                    var highlightDirectories = Directory.GetDirectories(HIGHLIGHTS_BASE_PATH, "h-*");
                    var highlightNames = highlightDirectories.Select(dir => 
                    {
                        var dirName = Path.GetFileName(dir);
                        return dirName.Substring(2); // Remove 'h-' prefix
                    }).ToList();

                    // Add highlights from the directory to the allowed list
                    availableHighlights.AddRange(highlightNames);
                    availableHighlights = availableHighlights.Distinct().ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading highlights directory: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Highlights base path does not exist: {HIGHLIGHTS_BASE_PATH}");
            }

            string currentPath = parentPath;

            // Check each item against available highlights
            foreach (var item in items)
            {
                var matchingHighlight = availableHighlights.FirstOrDefault(highlight => 
                    highlight.Equals(item, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrEmpty(matchingHighlight))
                {
                    var highlightPath = Path.Combine(currentPath, $"h-{matchingHighlight}");
                    try
                    {
                        Directory.CreateDirectory(highlightPath);
                        Console.WriteLine($"Created highlight directory: {highlightPath}");
                        currentPath = highlightPath;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to create highlight directory {highlightPath}: {ex.Message}");
                    }
                }
            }

            return currentPath;
        }
        
        /// <summary>
        /// Creates a title directory with 't-' prefix
        /// </summary>
        private static string CreateTitleDirectory(string parentPath, string title)
        {
            if (string.IsNullOrEmpty(parentPath) || string.IsNullOrEmpty(title))
                return parentPath;
                
            // Sanitize the title for use as a directory name
            var sanitizedTitle = SanitizeDirectoryName(title);
            var titlePath = Path.Combine(parentPath, $"t-{sanitizedTitle}");
            
            try
            {
                Directory.CreateDirectory(titlePath);
                Console.WriteLine($"Created title directory: {titlePath}");
                return titlePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create title directory {titlePath}: {ex.Message}");
                return parentPath;
            }
        }
        
        /// <summary>
        /// Sanitizes a string for use as a directory name
        /// </summary>
        private static string SanitizeDirectoryName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return "Unknown";
                
            // Remove or replace invalid directory name characters
            var invalidChars = Path.GetInvalidFileNameChars().Union(Path.GetInvalidPathChars()).ToArray();
            foreach (var invalidChar in invalidChars)
            {
                name = name.Replace(invalidChar, '_');
            }
            
            // Replace multiple spaces with single space and trim
            name = System.Text.RegularExpressions.Regex.Replace(name, @"\s+", " ").Trim();
            
            // Limit directory name length to avoid path length issues
            if (name.Length > 100)
            {
                name = name.Substring(0, 100).Trim();
            }
            
            return name;
        }
        
        /// <summary>
        /// Downloads and saves images to the specified directory
        /// </summary>
        public static async Task<int> SaveImageUrlsToDirectory(List<string> imageUrls, string directoryPath)
        {
            if (imageUrls == null || !imageUrls.Any() || string.IsNullOrEmpty(directoryPath))
            {
                Console.WriteLine("No image URLs or directory path provided");
                return 0;
            }
            
            try
            {
                Directory.CreateDirectory(directoryPath);
                Console.WriteLine($"Saving {imageUrls.Count} images to: {directoryPath}");
                
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("User-Agent", 
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
                
                int savedCount = 0;
                int imageIndex = 1;
                
                foreach (var imageUrl in imageUrls)
                {
                    try
                    {
                        var fileName = GenerateFileName(imageUrl, imageIndex);
                        var filePath = Path.Combine(directoryPath, fileName);
                        
                        if (File.Exists(filePath))
                        {
                            Console.WriteLine($"[{imageIndex}/{imageUrls.Count}] Skipped (already exists): {fileName}");
                            savedCount++;
                            imageIndex++;
                            continue;
                        }
                        
                        Console.WriteLine($"[{imageIndex}/{imageUrls.Count}] Downloading: {fileName}");
                        
                        var response = await httpClient.GetAsync(imageUrl);
                        if (response.IsSuccessStatusCode)
                        {
                            var imageData = await response.Content.ReadAsByteArrayAsync();
                            await File.WriteAllBytesAsync(filePath, imageData);
                            
                            Console.WriteLine($"[{imageIndex}/{imageUrls.Count}] Saved: {fileName} ({FormatFileSize(imageData.Length)})");
                            savedCount++;
                        }
                        else
                        {
                            Console.WriteLine($"[{imageIndex}/{imageUrls.Count}] Failed to download: {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[{imageIndex}/{imageUrls.Count}] Error downloading image: {ex.Message}");
                    }
                    
                    imageIndex++;
                }
                
                Console.WriteLine($"Successfully saved {savedCount} of {imageUrls.Count} images");
                return savedCount;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving images: {ex.Message}");
                return 0;
            }
        }
        
        /// <summary>
        /// Generates a filename from the image URL and index
        /// </summary>
        private static string GenerateFileName(string imageUrl, int index)
        {
            try
            {
                var uri = new Uri(imageUrl);
                var originalFileName = Path.GetFileName(uri.AbsolutePath);
                
                if (string.IsNullOrEmpty(originalFileName) || !HasValidImageExtension(originalFileName))
                {
                    originalFileName = $"image_{index:D4}.jpg";
                }
                
                return $"{index:D4}_{SanitizeFileName(originalFileName)}";
            }
            catch
            {
                return $"image_{index:D4}.jpg";
            }
        }
        
        /// <summary>
        /// Sanitizes a filename by removing invalid characters
        /// </summary>
        private static string SanitizeFileName(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            foreach (var invalidChar in invalidChars)
            {
                fileName = fileName.Replace(invalidChar, '_');
            }
            return fileName;
        }
        
        /// <summary>
        /// Checks if the filename has a valid image extension
        /// </summary>
        private static bool HasValidImageExtension(string fileName)
        {
            var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".svg", ".ico" };
            var extension = Path.GetExtension(fileName).ToLower();
            return imageExtensions.Contains(extension);
        }
        
        /// <summary>
        /// Formats file size in human-readable format
        /// </summary>
        private static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }
}