namespace GalleriaService.Api.Features;

public static class Services
{
    // add static methods for each service and execute method
    public static async Task ExecuteFileInfoGenerationAsync()
    {
        var service = new FileManagement.GenerateFileInfo.Service();
        await service.ExecuteAsync();
    }
    public static async Task ExecuteTagCloudCreationAsync()
    {
        var service = new TagCloud.CreateTagCloud.CreateTagCloud();
        await service.ExecuteAsync();
    }
    public static async Task ExecuteDuplicateIdentificationAsync()
    {
        var metadataFilePath = @"I:\.files.metadata.json";
        var service = new FileManagement.IdentifyDuplicates.DuplicateIdentifier(metadataFilePath);
        await service.ProcessAllAsync();
    }
}