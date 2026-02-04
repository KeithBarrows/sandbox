using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ImageScraper
{
    public class HtmlGalleryParser
    {
        public class GalleryInfo
        {
            public string Channel { get; set; } = string.Empty;
            public List<string> Models { get; set; } = new List<string>();
            public List<string> Categories { get; set; } = new List<string>();
            public List<string> TagsList { get; set; } = new List<string>();
            public List<string> ImageUrls { get; set; } = new List<string>();
            public string Title { get; set; } = string.Empty;
        }

        /// <summary>
        /// Parses HTML content and extracts gallery information including Channel, Models, Categories, and Tags List
        /// </summary>
        /// <param name="htmlContent">The HTML content to parse</param>
        /// <returns>GalleryInfo object containing all extracted information</returns>
        public static GalleryInfo ParseGalleryInfo(string htmlContent)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);
            
            var galleryInfo = new GalleryInfo();

            // Find the main gallery-info container
            var galleryInfoContainer = doc.DocumentNode
                .SelectSingleNode("//div[contains(@class, 'gallery-info')]");

            if (galleryInfoContainer == null)
                return galleryInfo;

            // Extract Channel information
            galleryInfo.Channel = ExtractChannelInfo(galleryInfoContainer);

            // Extract Models information
            galleryInfo.Models = ExtractModelsInfo(galleryInfoContainer);

            // Extract Categories information
            galleryInfo.Categories = ExtractCategoriesInfo(galleryInfoContainer);

            // Extract Tags List information
            galleryInfo.TagsList = ExtractTagsListInfo(galleryInfoContainer);

            // Extract Image URLs from thumbwook elements
            galleryInfo.ImageUrls = ExtractImageUrls(doc.DocumentNode);

            // Extract Title from the title element
            galleryInfo.Title = ExtractTitle(doc.DocumentNode);

            return galleryInfo;
        }

        /// <summary>
        /// Extracts Channel information from the gallery info container
        /// </summary>
        private static string ExtractChannelInfo(HtmlNode galleryInfoContainer)
        {
            // Look for the item with "Channel:" title
            var channelItem = galleryInfoContainer
                .SelectSingleNode(".//div[@class='gallery-info__item']//span[@class='gallery-info__title'][contains(text(), 'Channel:')]");
            
            if (channelItem != null)
            {
                // Get the parent div and find the anchor tag
                var parentDiv = channelItem.ParentNode;
                var anchorTag = parentDiv?.SelectSingleNode(".//a");
                
                if (anchorTag != null)
                {
                    return anchorTag.InnerText.Trim();
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Extracts Models information from the gallery info container
        /// </summary>
        private static List<string> ExtractModelsInfo(HtmlNode galleryInfoContainer)
        {
            var models = new List<string>();

            // Look for the item with "Models:" title
            var modelsItem = galleryInfoContainer
                .SelectSingleNode(".//div[@class='gallery-info__item']//span[@class='gallery-info__title'][contains(text(), 'Models:')]");

            if (modelsItem != null)
            {
                // Get the parent div and find the gallery-info__content div
                var parentDiv = modelsItem.ParentNode;
                var contentDiv = parentDiv?.SelectSingleNode(".//div[@class='gallery-info__content']");

                if (contentDiv != null)
                {
                    // Find all anchor tags with span children
                    var modelLinks = contentDiv.SelectNodes(".//a[span]");
                    
                    if (modelLinks != null)
                    {
                        foreach (var link in modelLinks)
                        {
                            var spanElement = link.SelectSingleNode(".//span");
                            if (spanElement != null)
                            {
                                var modelName = spanElement.InnerText.Trim();
                                if (!string.IsNullOrEmpty(modelName))
                                {
                                    models.Add(modelName);
                                }
                            }
                        }
                    }
                }
            }

            return models;
        }

        /// <summary>
        /// Extracts Categories information from the gallery info container
        /// </summary>
        private static List<string> ExtractCategoriesInfo(HtmlNode galleryInfoContainer)
        {
            var categories = new List<string>();

            // Look for the item with "Categories:" title
            var categoriesItem = galleryInfoContainer
                .SelectSingleNode(".//div[@class='gallery-info__item tags']//span[@class='gallery-info__title'][contains(text(), 'Categories:')]");

            if (categoriesItem != null)
            {
                // Get the parent div and find the gallery-info__content div
                var parentDiv = categoriesItem.ParentNode;
                var contentDiv = parentDiv?.SelectSingleNode(".//div[@class='gallery-info__content']");

                if (contentDiv != null)
                {
                    // Find all anchor tags with span children (excluding suggestion buttons)
                    var categoryLinks = contentDiv.SelectNodes(".//a[span and not(contains(@class, 'suggest'))]");
                    
                    if (categoryLinks != null)
                    {
                        foreach (var link in categoryLinks)
                        {
                            var spanElement = link.SelectSingleNode(".//span");
                            if (spanElement != null)
                            {
                                var categoryName = spanElement.InnerText.Trim();
                                if (!string.IsNullOrEmpty(categoryName))
                                {
                                    categories.Add(categoryName);
                                }
                            }
                        }
                    }
                }
            }

            return categories;
        }

        /// <summary>
        /// Extracts Tags List information from the gallery info container
        /// </summary>
        private static List<string> ExtractTagsListInfo(HtmlNode galleryInfoContainer)
        {
            var tagsList = new List<string>();

            // Look for the item with "Tags List:" title
            var tagsListItem = galleryInfoContainer
                .SelectSingleNode(".//div[@class='gallery-info__item tags']//span[@class='gallery-info__title'][contains(text(), 'Tags List:')]");

            if (tagsListItem != null)
            {
                // Get the parent div and find the gallery-info__content div
                var parentDiv = tagsListItem.ParentNode;
                var contentDiv = parentDiv?.SelectSingleNode(".//div[@class='gallery-info__content']");

                if (contentDiv != null)
                {
                    // Find all anchor tags with span children
                    var tagLinks = contentDiv.SelectNodes(".//a[span]");
                    
                    if (tagLinks != null)
                    {
                        foreach (var link in tagLinks)
                        {
                            var spanElement = link.SelectSingleNode(".//span");
                            if (spanElement != null)
                            {
                                var tagName = spanElement.InnerText.Trim();
                                if (!string.IsNullOrEmpty(tagName))
                                {
                                    tagsList.Add(tagName);
                                }
                            }
                        }
                    }
                }
            }

            return tagsList;
        }

        /// <summary>
        /// Extracts image URLs from anchor tags inside li elements with class 'thumbwook'
        /// </summary>
        /// <param name="documentNode">The root document node to search in</param>
        /// <returns>List of image URLs found in thumbwook elements</returns>
        private static List<string> ExtractImageUrls(HtmlNode documentNode)
        {
            var imageUrls = new List<string>();

            // Find all li elements with class 'thumbwook'
            var thumbwookElements = documentNode.SelectNodes("//li[contains(@class, 'thumbwook')]");

            if (thumbwookElements != null)
            {
                foreach (var thumbwookElement in thumbwookElements)
                {
                    // Find all anchor tags within this thumbwook element
                    var anchorTags = thumbwookElement.SelectNodes(".//a[@href]");

                    if (anchorTags != null)
                    {
                        foreach (var anchor in anchorTags)
                        {
                            var href = anchor.GetAttributeValue("href", "");
                            if (!string.IsNullOrEmpty(href))
                            {
                                // Add the href to the images list
                                imageUrls.Add(href.Trim());
                            }
                        }
                    }
                }
            }

            // Remove duplicates and return
            return imageUrls.Distinct().ToList();
        }

        /// <summary>
        /// Extracts the title from the HTML title element, up to the first dash
        /// </summary>
        /// <param name="documentNode">The root document node to search in</param>
        /// <returns>Title text up to the first dash</returns>
        private static string ExtractTitle(HtmlNode documentNode)
        {
            try
            {
                // Find the title element in the head section
                var titleElement = documentNode.SelectSingleNode("//head/title");
                
                if (titleElement != null)
                {
                    var fullTitle = titleElement.InnerText?.Trim();
                    if (!string.IsNullOrEmpty(fullTitle))
                    {
                        // Extract text up to the first dash
                        var dashIndex = fullTitle.IndexOf(" - ");
                        if (dashIndex > 0)
                        {
                            return fullTitle.Substring(0, dashIndex).Trim();
                        }
                        
                        // If no dash found, return the full title
                        return fullTitle;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting title: {ex.Message}");
            }

            return string.Empty;
        }

        /// <summary>
        /// Reads HTML content from a file and parses gallery information
        /// </summary>
        /// <param name="filePath">Path to the HTML file</param>
        /// <returns>GalleryInfo object containing extracted information</returns>
        public static GalleryInfo ParseGalleryInfoFromFile(string filePath)
        {
            try
            {
                var htmlContent = File.ReadAllText(filePath);
                return ParseGalleryInfo(htmlContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file {filePath}: {ex.Message}");
                return new GalleryInfo();
            }
        }

        /// <summary>
        /// Downloads HTML content from a URL and parses gallery information
        /// </summary>
        /// <param name="url">URL to download HTML from</param>
        /// <returns>GalleryInfo object containing extracted information</returns>
        public static async Task<GalleryInfo> ParseGalleryInfoFromUrlAsync(string url)
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("User-Agent", 
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
                
                var htmlContent = await httpClient.GetStringAsync(url);
                return ParseGalleryInfo(htmlContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading from URL {url}: {ex.Message}");
                return new GalleryInfo();
            }
        }
    }
}