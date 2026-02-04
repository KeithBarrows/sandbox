using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageScraper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Image Scraper Application");
            Console.WriteLine("========================\n");

            if (args.Length == 0)
            {
                // Run the gallery parser demo if no arguments provided
                await GalleryParserDemo.RunDemoAsync();
                
                Console.WriteLine("\n\nPress any key to continue with main menu...");
                Console.ReadKey();
                Console.Clear();
                
                // Show the main menu
                await ShowMainMenuAsync();
            }
            else
            {
                // Run with command line arguments for gallery parsing
                string filePath = args[0];
                await RunGalleryParserAsync(filePath);
            }
        }

        static async Task ShowMainMenuAsync()
        {
            while (true)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Parse gallery information from HTML file");
                Console.WriteLine("2. Parse gallery information from URL");
                Console.WriteLine("3. Show individual lists example");
                Console.WriteLine("4. Organize gallery and download images");
                Console.WriteLine("5. Exit");
                Console.Write("\nEnter your choice (1-5): ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter HTML file path: ");
                        var filePath = Console.ReadLine();
                        if (!string.IsNullOrEmpty(filePath))
                        {
                            await RunGalleryParserAsync(filePath);
                        }
                        break;
                    case "2":
                        Console.Write("Enter URL to parse: ");
                        var url = Console.ReadLine();
                        if (!string.IsNullOrEmpty(url))
                        {
                            await RunUrlParserAsync(url);
                        }
                        break;
                    case "3":
                        await ShowIndividualListsExampleAsync();
                        break;
                    case "4":
                        await OrganizeAndDownloadAsync();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static async Task RunGalleryParserAsync(string filePath)
        {
            try
            {
                Console.WriteLine($"Parsing gallery information from: {filePath}");
                Console.WriteLine(new string('-', 50));

                var galleryInfo = HtmlGalleryParser.ParseGalleryInfoFromFile(filePath);
                DisplayGalleryInfo(galleryInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task RunUrlParserAsync(string url)
        {
            try
            {
                Console.WriteLine($"Parsing gallery information from URL: {url}");
                Console.WriteLine(new string('-', 50));

                var galleryInfo = await HtmlGalleryParser.ParseGalleryInfoFromUrlAsync(url);
                DisplayGalleryInfo(galleryInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task ShowIndividualListsExampleAsync()
        {
            try
            {
                Console.WriteLine("Individual Lists Example using sample file:");
                Console.WriteLine(new string('-', 50));

                var sampleFilePath = "sample01.partial.htm";
                if (System.IO.File.Exists(sampleFilePath))
                {
                    var htmlContent = System.IO.File.ReadAllText(sampleFilePath);
                    GalleryParserDemo.GetIndividualLists(htmlContent);
                }
                else
                {
                    Console.WriteLine("Sample file not found. Creating example with mock data...");
                    
                    // Mock example
                    var mockGalleryInfo = new HtmlGalleryParser.GalleryInfo
                    {
                        Title = "Example Gallery Title",
                        Channel = "Example Channel",
                        Models = new List<string> { "Model 1", "Model 2" },
                        Categories = new List<string> { "Category 1", "Category 2", "Category 3" },
                        TagsList = new List<string> { "Tag 1", "Tag 2", "Tag 3", "Tag 4" },
                        ImageUrls = new List<string> { "/image1.jpg", "/image2.jpg", "/image3.jpg" }
                    };

                    List<string> titleList = new List<string> { mockGalleryInfo.Title };
                    List<string> channelList = new List<string> { mockGalleryInfo.Channel };
                    List<string> modelsList = mockGalleryInfo.Models;
                    List<string> categoriesList = mockGalleryInfo.Categories;
                    List<string> tagsListList = mockGalleryInfo.TagsList;
                    List<string> imageUrlsList = mockGalleryInfo.ImageUrls;

                    Console.WriteLine("Mock Individual Lists Example:");
                    Console.WriteLine($"Title List: [{string.Join(", ", titleList)}]");
                    Console.WriteLine($"Channel List: [{string.Join(", ", channelList)}]");
                    Console.WriteLine($"Models List: [{string.Join(", ", modelsList)}]");
                    Console.WriteLine($"Categories List: [{string.Join(", ", categoriesList)}]");
                    Console.WriteLine($"Tags List: [{string.Join(", ", tagsListList)}]");
                    Console.WriteLine($"Image URLs List: [{string.Join(", ", imageUrlsList)}]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static void DisplayGalleryInfo(HtmlGalleryParser.GalleryInfo galleryInfo)
        {
            Console.WriteLine($"Title: {galleryInfo.Title}");
            Console.WriteLine($"Channel: {galleryInfo.Channel}");
            Console.WriteLine();

            Console.WriteLine($"Models ({galleryInfo.Models.Count}):");
            foreach (var model in galleryInfo.Models)
            {
                Console.WriteLine($"  - {model}");
            }
            Console.WriteLine();

            Console.WriteLine($"Categories ({galleryInfo.Categories.Count}):");
            foreach (var category in galleryInfo.Categories)
            {
                Console.WriteLine($"  - {category}");
            }
            Console.WriteLine();

            Console.WriteLine($"Tags List ({galleryInfo.TagsList.Count}):");
            foreach (var tag in galleryInfo.TagsList)
            {
                Console.WriteLine($"  - {tag}");
            }
            Console.WriteLine();

            Console.WriteLine($"Image URLs ({galleryInfo.ImageUrls.Count}):");
            foreach (var imageUrl in galleryInfo.ImageUrls)
            {
                Console.WriteLine($"  - {imageUrl}");
            }
            Console.WriteLine();

            // Show individual lists format
            Console.WriteLine("Individual Lists Format:");
            Console.WriteLine("========================");
            List<string> titleList = new List<string> { galleryInfo.Title };
            List<string> channelList = new List<string> { galleryInfo.Channel };
            List<string> modelsList = galleryInfo.Models;
            List<string> categoriesList = galleryInfo.Categories;
            List<string> tagsListList = galleryInfo.TagsList;
            List<string> imageUrlsList = galleryInfo.ImageUrls;

            Console.WriteLine($"Title List: [{string.Join(", ", titleList)}]");
            Console.WriteLine($"Channel List: [{string.Join(", ", channelList)}]");
            Console.WriteLine($"Models List: [{string.Join(", ", modelsList)}]");
            Console.WriteLine($"Categories List: [{string.Join(", ", categoriesList)}]");
            Console.WriteLine($"Tags List: [{string.Join(", ", tagsListList)}]");
            Console.WriteLine($"Image URLs List: [{string.Join(", ", imageUrlsList)}]");
        }

        static async Task OrganizeAndDownloadAsync()
        {
            try
            {
                Console.Write("Enter HTML file path or URL: ");
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("No input provided.");
                    return;
                }

                HtmlGalleryParser.GalleryInfo galleryInfo;

                // Determine if input is a file path or URL
                if (input.StartsWith("http://") || input.StartsWith("https://"))
                {
                    Console.WriteLine($"Parsing gallery information from URL: {input}");
                    galleryInfo = await HtmlGalleryParser.ParseGalleryInfoFromUrlAsync(input);
                }
                else
                {
                    Console.WriteLine($"Parsing gallery information from file: {input}");
                    galleryInfo = HtmlGalleryParser.ParseGalleryInfoFromFile(input);
                }

                if (string.IsNullOrEmpty(galleryInfo.Channel) && 
                    !galleryInfo.Models.Any() && 
                    !galleryInfo.ImageUrls.Any())
                {
                    Console.WriteLine("No gallery information found or parsed.");
                    return;
                }

                // Display parsed information
                Console.WriteLine();
                Console.WriteLine("Parsed Gallery Information:");
                Console.WriteLine(new string('-', 40));
                DisplayGalleryInfo(galleryInfo);

                // Check if channel contains Penthouse or Playboy
                bool isPenthouseOrPlayboy = galleryInfo.Channel.Contains("Penthouse", StringComparison.OrdinalIgnoreCase) ||
                                          galleryInfo.Channel.Contains("Playboy", StringComparison.OrdinalIgnoreCase);

                if (!isPenthouseOrPlayboy)
                {
                    Console.WriteLine($"Channel '{galleryInfo.Channel}' is not Penthouse or Playboy. Skipping directory organization.");
                    return;
                }

                // Organize directory structure
                Console.WriteLine();
                Console.WriteLine("Organizing directory structure...");
                Console.WriteLine(new string('-', 40));

                string finalDirectory = DirectoryOrganizer.OrganizeGalleryDirectories(galleryInfo);

                if (string.IsNullOrEmpty(finalDirectory))
                {
                    Console.WriteLine("Failed to organize directory structure.");
                    return;
                }

                // Ask user if they want to download images
                Console.WriteLine();
                Console.Write($"Download {galleryInfo.ImageUrls.Count} images to '{finalDirectory}'? (y/N): ");
                var response = Console.ReadLine()?.Trim().ToLower();

                if (response == "y" || response == "yes")
                {
                    Console.WriteLine();
                    Console.WriteLine("Downloading images...");
                    Console.WriteLine(new string('-', 40));

                    int savedCount = await DirectoryOrganizer.SaveImageUrlsToDirectory(galleryInfo.ImageUrls, finalDirectory);
                    
                    Console.WriteLine();
                    Console.WriteLine($"Download completed! {savedCount} of {galleryInfo.ImageUrls.Count} images saved to:");
                    Console.WriteLine(finalDirectory);
                }
                else
                {
                    Console.WriteLine("Download cancelled. Directory structure created but no images downloaded.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
