using System;
using System.IO;
using System.Threading.Tasks;

namespace ImageScraper
{
    public class GalleryParserDemo
    {
        public static async Task RunDemoAsync()
        {
            Console.WriteLine("=== HTML Gallery Parser Demo ===\n");

            // Test with the sample file
            var sampleFilePath = "sample01.partial.htm";
            
            if (File.Exists(sampleFilePath))
            {
                Console.WriteLine($"Parsing gallery info from file: {sampleFilePath}");
                Console.WriteLine(new string('-', 50));
                
                var galleryInfo = HtmlGalleryParser.ParseGalleryInfoFromFile(sampleFilePath);
                
                DisplayGalleryInfo(galleryInfo);
            }
            else
            {
                Console.WriteLine($"Sample file {sampleFilePath} not found.");
            }

            // Example of parsing from a URL (commented out for safety)
            /*
            Console.WriteLine("\n\nParsing gallery info from URL...");
            Console.WriteLine(new string('-', 50));
            
            var url = "https://example.com/gallery-page";
            var galleryInfoFromUrl = await HtmlGalleryParser.ParseGalleryInfoFromUrlAsync(url);
            DisplayGalleryInfo(galleryInfoFromUrl);
            */
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
        }

        /// <summary>
        /// Alternative method to get individual lists separately
        /// </summary>
        public static void GetIndividualLists(string htmlContent)
        {
            var galleryInfo = HtmlGalleryParser.ParseGalleryInfo(htmlContent);

            // Access individual lists
            List<string> titleList = new List<string> { galleryInfo.Title };
            List<string> channelList = new List<string> { galleryInfo.Channel };
            List<string> modelsList = galleryInfo.Models;
            List<string> categoriesList = galleryInfo.Categories;
            List<string> tagsListList = galleryInfo.TagsList;
            List<string> imageUrlsList = galleryInfo.ImageUrls;

            // Use the lists as needed
            Console.WriteLine("Individual Lists Example:");
            Console.WriteLine($"Title List: [{string.Join(", ", titleList)}]");
            Console.WriteLine($"Channel List: [{string.Join(", ", channelList)}]");
            Console.WriteLine($"Models List: [{string.Join(", ", modelsList)}]");
            Console.WriteLine($"Categories List: [{string.Join(", ", categoriesList)}]");
            Console.WriteLine($"Tags List: [{string.Join(", ", tagsListList)}]");
            Console.WriteLine($"Image URLs List: [{string.Join(", ", imageUrlsList)}]");
        }
    }
}