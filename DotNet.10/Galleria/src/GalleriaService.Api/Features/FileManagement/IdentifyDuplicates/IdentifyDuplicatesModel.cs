using System;
using System.Collections.Generic;
using System.Text.Json;

namespace GalleriaService.Api.Features.FileManagement.IdentifyDuplicates;

public class IdentifyDuplicatesModel
{
    private const string FileName = @"I:\.duplicates.json";

    public List<PathPairDto> PathPairs { get; set; } = [];
    public List<TagPathJoinDto> TagPathJoins { get; set; } = [];

    public async Task AddOrUpdate(string tag, string filePath1, string filePath2)
    {
        var pair = new Tuple<string, string>(filePath1, filePath2);
        var pathPairId = Guid.NewGuid();
        var pathPair = new PathPairDto(pathPairId, pair);

        // Check if the pair already exists
        var existingPair = PathPairs.FirstOrDefault(x => x.FullPathPair == pair);
        if (existingPair != null)
            pathPair = existingPair;
        else
            PathPairs.Add(pathPair);

        // Add or update tag association
        var existingTag = TagPathJoins.FirstOrDefault(x => x.Tag == tag && x.PathPair == pathPair);
        if (existingTag == null)
            TagPathJoins.Add(new TagPathJoinDto(tag, pathPair));
        
        await WriteToDisc();
    }

    internal async Task WriteToDisc()
    {
        // write TagPathJoinDtos to disc as json
        using var stream = new FileStream(FileName, FileMode.Create);
        await JsonSerializer.SerializeAsync(stream, TagPathJoins);
    }
    internal async Task ReadFromDisc()
    {
        if (!File.Exists(FileName))
            return;

        using var stream = new FileStream(FileName, FileMode.Open);
        var tagPathJoins = await JsonSerializer.DeserializeAsync<List<TagPathJoinDto>>(stream);
        if (tagPathJoins == null)
            return;

        TagPathJoins = tagPathJoins;

        // Rebuild PathPairs from TagPathJoins
        PathPairs = TagPathJoins.Select(x => x.PathPair).Distinct().ToList();
    }



    public class PathPairDto(Guid pathPairId, Tuple<string, string> fullPathPair)
    {
        public Guid PathPairId { get; set; } = pathPairId;
        public Tuple<string, string> FullPathPair { get; set; } = fullPathPair;
    }

    public class TagPathJoinDto(string tag, PathPairDto pathPair)
    {
        public string Tag { get; set; } = tag;
        public PathPairDto PathPair { get; set; } = pathPair;
    }
}
