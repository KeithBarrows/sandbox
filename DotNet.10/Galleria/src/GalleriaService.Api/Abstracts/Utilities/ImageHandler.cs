using SixLabors.ImageSharp;
using System.IO;

namespace GalleriaService.Api.Abstracts.Utilities;

public static class ImageValidator
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string Format { get; set; } = string.Empty;
    }

    public static ValidationResult ValidateImage(string filePath)
    {
        var result = new ValidationResult();

        try
        {
            // Check if file exists and has content
            var fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists || fileInfo.Length == 0)
            {
                result.ErrorMessage = "File does not exist or is empty";
                return result;
            }

            result.FileSize = fileInfo.Length;

            // Try to load and decode the image
            using var image = Image.Load(filePath);

            // Basic sanity checks
            if (image.Width == 0 || image.Height == 0)
            {
                result.ErrorMessage = "Invalid image dimensions";
                return result;
            }

            result.IsValid = true;
            result.Format = image.Metadata.DecodedImageFormat?.Name ?? "Unknown";
            return result;
        }
        catch (UnknownImageFormatException)
        {
            result.ErrorMessage = "Unknown or unsupported image format";
            return result;
        }
        catch (InvalidImageContentException)
        {
            result.ErrorMessage = "Image content is corrupted";
            return result;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = $"Validation failed: {ex.Message}";
            return result;
        }
    }
}




/*
 * Usage example
 *

var result = ImageValidator.ValidateImage(@"C:\path\to\image.jpg");
if (!result.IsValid)
{
    Console.WriteLine($"Image is corrupt: {result.ErrorMessage}");
    Console.WriteLine($"File size: {result.FileSize} bytes");
    Console.WriteLine($"Format: {result.Format}");
}

 *
 *
 */