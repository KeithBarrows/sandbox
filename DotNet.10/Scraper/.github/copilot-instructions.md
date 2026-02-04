# Image Scraper Project - Copilot Instructions

This is a comprehensive C# console application for web image downloading that parses HTML gallery pages, extracts metadata, and downloads images with sophisticated directory organization.

## Project Completed ✅

- [x] **Project Requirements**: C# console application for web image downloader tool that parses HTML pages, extracts image URLs, and downloads images to local folder
- [x] **Project Scaffolding**: Created C# console application with ImageScraper.csproj, Program.cs, HtmlGalleryParser.cs, DirectoryOrganizer.cs  
- [x] **Project Customization**: Developed a comprehensive image scraper with HTML parsing, concurrent downloading, and user-friendly interface
- [x] **Extensions**: No additional extensions required for this console application
- [x] **Compilation**: Project successfully compiled with no errors or warnings. NuGet packages restored.
- [x] **Task Creation**: Build and run tasks available via `dotnet run` command
- [x] **Launch Ready**: Project is fully functional and ready to run
- [x] **Documentation**: README.md and copilot-instructions.md files are complete and current

## Key Features

- **HTML Gallery Parsing**: Extracts Channel, Models, Categories, Tags, Image URLs, and Title from gallery pages
- **Directory Organization**: Complex multi-level directory structure with studio, gallery, person, category, highlight, and title organization
- **Multi-Directory Processing**: Supports Album (a-), Highlight (h-), and Category (c-) directory prefixes
- **Image Downloading**: Concurrent download with progress tracking and error handling
- **File Management**: Automatic filename sanitization and duplicate handling

## Technical Stack

- **.NET 8.0**: Modern C# console application
- **HtmlAgilityPack 1.11.54**: HTML parsing and XPath selectors
- **HttpClient**: Web content downloading with proper headers
- **Async/Await**: Non-blocking operations for better performance

## Usage

Run the application with `dotnet run` and follow the interactive menu to parse gallery URLs and download images with organized directory structures.