using System.Drawing;

const int TargetWidth = 3440;
const int TargetHeight = 1440;

string[] sourceDirs =
[
    @"E:\OneDrive\Pictures\2-Wallpaper - 1 Screen",
    @"E:\OneDrive\Pictures\3-Wallpaper - 2 Screen",
    @"E:\OneDrive\Pictures\4-Wallpaper - 3 Screen",
];

string destDir = @"E:\OneDrive\Pictures\2-Wallpaper (3440x1440)";

string[] imageExtensions = [".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".tif", ".webp"];

Directory.CreateDirectory(destDir);

int moved = 0;
int skipped = 0;
int errors = 0;

foreach (string sourceDir in sourceDirs)
{
    if (!Directory.Exists(sourceDir))
    {
        Console.WriteLine($"[SKIP] Directory not found: {sourceDir}");
        continue;
    }

    Console.WriteLine($"\nScanning: {sourceDir}");

    var files = Directory.EnumerateFiles(sourceDir, "*", SearchOption.AllDirectories)
        .Where(f => imageExtensions.Contains(Path.GetExtension(f).ToLowerInvariant()));

    foreach (string file in files)
    {
        try
        {
            using var img = Image.FromFile(file);
            if (img.Width != TargetWidth || img.Height != TargetHeight)
            {
                skipped++;
                continue;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  [ERROR] Cannot read image: {file}\n         {ex.Message}");
            errors++;
            continue;
        }

        string fileName = Path.GetFileName(file);
        string destPath = Path.Combine(destDir, fileName);

        // Avoid collisions by appending a counter suffix
        if (File.Exists(destPath))
        {
            string nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            string ext = Path.GetExtension(fileName);
            int counter = 1;
            do
            {
                destPath = Path.Combine(destDir, $"{nameWithoutExt}_{counter++}{ext}");
            } while (File.Exists(destPath));
        }

        try
        {
            File.Move(file, destPath);
            Console.WriteLine($"  [MOVED] {file}\n      --> {destPath}");
            moved++;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  [ERROR] Could not move: {file}\n         {ex.Message}");
            errors++;
        }
    }
}

Console.WriteLine($"""

Done.
  Moved:   {moved}
  Skipped: {skipped} (wrong size)
  Errors:  {errors}
""");
