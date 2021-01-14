using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Scraper
{
    class Program
    {
        static void Main()
        {
            string dirPath = @"I:\IO\Album\a-Cosplay\p-Octokuro\Scrape";

            // Create a new directory
            _ = Directory.CreateDirectory(dirPath);
            Console.WriteLine($"Directory '{Path.GetFileName(dirPath)}' was created successfully in {Directory.GetParent(dirPath).FullName}");

            //var imageList = ExtractAllImages(@"https://www.pichunter.com/models/Sandee_Westgate/photos/11").ToList();
            //var imageList = ExtractAllImages(@"https://www.coedcherry.com/models/katsuni/pics/asian-girl-katsuni-getting-pussy-pounded").ToList();
            //var imageList = ExtractAllImages(@"https://babesrater.com/infinite-scroll/79403/nicole-marie-lenz").ToList();
            //var imageList = ExtractAllImages(@"https://pholder.com/u/azuracosplay/").ToList();
            //var imageList = ExtractAllImages(@"https://nudecosplaygirls.com/kalinka-fox-nude/").ToList();
            //var imageList = ExtractAllImages(@"https://prothots.com/kalinka-fox-naked-hinata-cosplay-nude-photos/").ToList();
            //var imageList = ExtractAllImages(@"https://www.erome.com/a/ncvrQH49").ToList();
            var imageList = ExtractAllImages(@"https://realpornclip.com/octokuro-nude-cosplay-photos-62-pics/").ToList();

            foreach (var imagePath in imageList)
            {
                try
                {
                    if (!imagePath.ToUpper().StartsWith("HTTP"))
                        continue;

                    using WebClient _wc = new WebClient();
                    var index = imagePath.IndexOf("?");
                    string fileName = imagePath;
                    if (index > 0)
                        fileName = imagePath.Substring(0, index);
                    fileName = Path.GetFileName(fileName).Replace("?ssl=1", "");

                    var fileParts = fileName.Split('.');
                    var ext = fileParts[1];

                    if (int.TryParse(fileParts[0], out int fileNumber))
                    {
                        fileNumber += 100;
                        fileName = $"{fileNumber:000}.{ext}";
                    }

                    //var path = imagePath.Replace($"/{fileName}", "");
                    //var pathParts = path.Split(new[] { '/' });
                    //var fileParts = fileName.Split(new[] { '.' });
                    //var lastPath = pathParts[pathParts.Length - 1];
                    //fileName = $"{fileParts[0]}-{lastPath}.{fileParts[1]}";

                    _wc.DownloadFileAsync(new Uri(imagePath), Path.Combine(dirPath, $"{fileName}"));
                    var isDone = false;
                    while (!isDone)
                    {
                        isDone = !_wc.IsBusy;
                    }
                }

                catch (Exception e)
                {
                    while (e != null)
                    {
                        Console.WriteLine(e.Message);
                        e = e.InnerException;
                    }
                }
            }

            Console.WriteLine("\nFile successfully saved.");

            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.WriteLine("Press any key to continue . . .");
                Console.ReadKey(true);
            }
        }

        static IEnumerable<string> ExtractAllImages(string webPageUrl)
        {

            // declare html document
            //var document = new HtmlWeb().Load("https://www.technologycrowds.com/2019/12/net-core-web-api-tutorial.html");
            var document = new HtmlWeb().Load(webPageUrl);

            // now using LINQ to grab/list all images from website
            var linkImages = document.DocumentNode.Descendants("a")
                                            .Select(e => e.GetAttributeValue("href", null))
                                            .Where(s => !String.IsNullOrEmpty(s)).ToList();

            var ImageURLs = linkImages.Where(a => a.EndsWith(".jpg") || a.EndsWith(".jpeg") || a.EndsWith(".png") || a.EndsWith(".webp") || a.EndsWith(".mp4")).ToList();

            if (ImageURLs.Count <= 0)
            {
                //ImageURLs = document.DocumentNode.Descendants("div")
                //                                .Select(e => e.GetAttributeValue("data-src", null)).ToList()
                //                                .Where(s => !String.IsNullOrEmpty(s)).ToList();

                ImageURLs = document.DocumentNode.Descendants("img")
                                                .Select(e => e.GetAttributeValue("data-orig-file", null)).ToList()
                                                .Where(s => !String.IsNullOrEmpty(s)).ToList();

                //ImageURLs = document.DocumentNode.Descendants("img")
                //                                .Select(e => e.GetAttributeValue("src", null)).ToList();
                ////.Where(s => !String.IsNullOrEmpty(s)).ToList();
                var VideoURLs = document.DocumentNode.Descendants("video")
                                                .Select(e => e.GetAttributeValue("src", null))
                                                .Where(s => !String.IsNullOrEmpty(s)).ToList();
                ImageURLs.AddRange(VideoURLs);
            }

            // now showing all images from web page one by one
            ImageURLs.Where(a => a != null).ToList().ForEach(a => Console.WriteLine(a));

            return ImageURLs;
        }
    }
}