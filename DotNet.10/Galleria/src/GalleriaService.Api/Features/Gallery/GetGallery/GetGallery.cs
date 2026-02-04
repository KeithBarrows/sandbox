using System;
using System.IO;
using System.Threading.Tasks;
using FFMpegCore;
using FFMpegCore.Pipes;

namespace GalleriaService.Api.Features.Gallery.GetGallery;

public static class VideoFrameExtractor
{
    /// <summary>
    /// Extracts a single frame from about the 10% mark of a video file and returns it as a PNG byte array.
    /// </summary>
    /// <param name="videoPath">The path to the video file.</param>
    /// <returns>PNG image bytes of the extracted frame.</returns>
    public static async Task<byte[]> ExtractFrameAt10PercentAsync(string videoPath)
    {
        if (!File.Exists(videoPath))
            throw new FileNotFoundException("Video file not found.", videoPath);

        // Get video duration using FFMpeg
        var mediaInfo = await FFProbe.AnalyseAsync(videoPath);
        var duration = mediaInfo.Duration;
        if (duration.TotalSeconds < 1)
            throw new InvalidOperationException("Video duration is too short.");

        var seek = duration.TotalSeconds * 0.10;
        using var ms = new MemoryStream();
        await FFMpegArguments
            .FromFileInput(videoPath)
            .AddInputArgument($"-ss {seek}")
            .OutputToPipe(new StreamPipeSink(ms), options => options
                .WithFrameOutputCount(1)
                .ForceFormat("image2")
                .WithVideoCodec("png"))
            .ProcessAsynchronously();
        return ms.ToArray();
    }
}
