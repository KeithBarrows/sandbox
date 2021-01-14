using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using NReco.PhantomJS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Scraper.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScraperController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public ScrapeModel Get(string sourceUrl, string targetDirs)
        {
            var dirPath = CreateTargetDestination(targetDirs);
            var imageList = ExtractAllImages(sourceUrl).ToList();
            SaveToDisk(imageList, dirPath);

            return new ScrapeModel { TargetDir = dirPath, Photos = imageList };
        }

        private string CreateTargetDestination(string targetDirs)
        {
            var targetList = targetDirs.Split('|', '~', ',', '.', '\\', '/');
            var rootPath = @"I:\IO";
            var dirPath = "";
            var dirPerson = "";
            var dirAlbum = "";
            var dirCategory = "";
            var dirHighlight = "";
            var dirStudio = "";
            var dirGallery = "";

            foreach (var dirPart in targetList)
            {
                if (dirPart.ToUpper().StartsWith("A-")) dirAlbum = $"{dirAlbum}\\{dirPart}";
                if (dirPart.ToUpper().StartsWith("C-")) dirCategory = $"{dirCategory}\\{dirPart}";
                if (dirPart.ToUpper().StartsWith("H-")) dirHighlight = $"{dirHighlight}\\{dirPart}";
                if (dirPart.ToUpper().StartsWith("P-")) dirPerson = $"{dirPerson}\\{dirPart}";
                if (dirPart.ToUpper().StartsWith("S-")) dirStudio = $"{dirStudio}\\{dirPart}";
                if (dirPart.ToUpper().StartsWith("G-")) dirGallery = $"{dirGallery}\\{dirPart}";
            }

            var sexAlbums = new[] { "a-Cam Girl", "a-FF", "a-FFF", "a-FFFF", "a-FFFM", "a-FFFMM", "a-FFM", "a-FFMM", "a-FM", "a-FMM", "a-Orgasm", "a-Orgy", "a-Porn Star", "a-Positions", "a-Solo" };
            if (dirAlbum.Length > 0 && !dirAlbum.Contains("a-Sex"))
            {
                var parts = dirAlbum.Split("\\");
                var isSex = false;
                foreach(var part in parts)
                {
                    if(sexAlbums.Contains(part) || isSex)
                        isSex = true;
                }
                if (isSex)
                    dirAlbum = $"\\a-Sex{dirAlbum}";
            }

            if (dirCategory.Length > 0)
                dirPath = $"{rootPath}\\Category{dirCategory}{dirPerson}{dirAlbum}{dirHighlight}{dirStudio}{dirGallery}";
            if (dirHighlight.Length > 0)
                dirPath = $"{rootPath}\\Highlight{dirHighlight}{dirPerson}{dirAlbum}{dirCategory}{dirStudio}{dirGallery}";
            if (dirPerson.Length > 0)
                dirPath = $"{rootPath}\\Person{dirPerson}{dirAlbum}{dirCategory}{dirHighlight}{dirStudio}{dirGallery}";
            if (dirAlbum.Length > 0)
                dirPath = $"{rootPath}\\Album{dirAlbum}{dirPerson}{dirCategory}{dirHighlight}{dirStudio}{dirGallery}";
            if (dirStudio.Length>0)
                dirPath = $"{rootPath}\\Studio{dirStudio}{dirAlbum}{dirPerson}{dirCategory}{dirHighlight}{dirGallery}";
            _ = Directory.CreateDirectory(dirPath);
            Console.WriteLine($"Directory '{Path.GetFileName(dirPath)}' was created successfully in {Directory.GetParent(dirPath).FullName}");

            return dirPath;
        }

        private IEnumerable<string> ExtractAllImages(string webPageUrl)
        {
            //var phantomJS = new PhantomJS();
            //phantomJS.Run()

            // declare html document
            //var document = new HtmlWeb().Load("https://www.technologycrowds.com/2019/12/net-core-web-api-tutorial.html");

            var document = new HtmlWeb().Load(webPageUrl);
            //var document = new HtmlDocument();
            //document.LoadHtml(result);
            var nodes = document.DocumentNode.SelectNodes("//div[@id='fullimages']//a//img[@src]");

            // now using LINQ to grab/list all images from website
            var linkImages = document.DocumentNode.Descendants("a")
                                            .Select(e => e.GetAttributeValue("href", null))
                                            .Where(s => !String.IsNullOrEmpty(s)).ToList();

            var ImageURLs = linkImages.Where(a => a.EndsWith(".jpg") || a.EndsWith(".jpeg") || a.EndsWith(".png") || a.EndsWith(".webp") || a.EndsWith(".mp4")).ToList();

            if (ImageURLs.Count <= 0)
            {
                ImageURLs = document.DocumentNode.Descendants("img")
                                                .Select(e => e.GetAttributeValue("src", null)).ToList();
                //.Where(s => !String.IsNullOrEmpty(s)).ToList();
                var VideoURLs = document.DocumentNode.Descendants("video")
                                                .Select(e => e.GetAttributeValue("src", null))
                                                .Where(s => !String.IsNullOrEmpty(s)).ToList();
                ImageURLs.AddRange(VideoURLs);
            }

            // now showing all images from web page one by one
            ImageURLs.Where(a => a != null).ToList().ForEach(a => Console.WriteLine(a));

            return ImageURLs;
        }

        private void SaveToDisk(List<string> imageList, string dirPath)
        {
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
        }
    }

    public class ScrapeModel
    {
        public string TargetDir { get; set; }
        public List<string> Photos { get; set; }
    }
}
