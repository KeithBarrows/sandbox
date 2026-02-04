using System;
using System.Collections.Generic;
using System.Text.Json;

namespace GalleriaService.Api.Features.FileManagement.IdentifyDuplicates;

public class IdentifyDuplicatesIndex
{
    private const string FileName = @"I:\.duplicates.index.json";

    public string LastTag { get; set; } = string.Empty;
    public int LastInnerIndex { get; set; } = -1;
    public int LastOuterIndex { get; set; } = -1;
    public Dictionary<string, bool> IsCompleted { get; set; } = [];
    public List<Entry> Entries { get; set; } = [];

    public async Task AddOrUpdate(Entry entry)
    {
        if (Entries.Any(e => e.Tag == entry.Tag && e.IndexOuter == entry.IndexOuter && e.IndexInner == entry.IndexInner && e.FileOuter == entry.FileOuter && e.FileInner == string.Empty))
        {
            var foundEntry = Entries.First(e => e.Tag == entry.Tag && e.IndexOuter == entry.IndexOuter && e.IndexInner == entry.IndexInner && e.FileOuter == entry.FileOuter && e.FileInner == entry.FileInner);
            entry.FileInner = foundEntry.FileInner;
        }
        else
        {
            Entries.Add(entry);
        }
        LastTag = entry.Tag;
        LastInnerIndex = entry.IndexInner;
        LastOuterIndex = entry.IndexOuter;

        await WriteToDisc();
    }

    public async Task CompleteTag(string tag)
    {
        if (IsCompleted.ContainsKey(tag))
            IsCompleted[tag] = true;
        else
            IsCompleted.Add(tag, true);
        LastTag = tag;
        var entry = Entries.Where(x => x.Tag == tag)
                           .OrderByDescending(x => x.IndexOuter)
                           .ThenByDescending(x => x.IndexInner)
                           .FirstOrDefault();
        if (entry != null)
        {
            LastOuterIndex = entry.IndexOuter;
            LastInnerIndex = entry.IndexInner;
        }

        await WriteToDisc();
    }

    private async Task WriteToDisc()
    {
        // write TagPathJoinDtos to disc as json
        using var stream = new FileStream(FileName, FileMode.Create);
        await JsonSerializer.SerializeAsync(stream, this);
    }
    internal async Task ReadFromDisc()
    {
        if (!File.Exists(FileName))
            return;

        using var stream = new FileStream(FileName, FileMode.Open);
        var index = await JsonSerializer.DeserializeAsync<IdentifyDuplicatesIndex>(stream);
        if (index == null)
            return;

        LastTag = index.LastTag;
        LastInnerIndex = index.LastInnerIndex;
        LastOuterIndex = index.LastOuterIndex;
        Entries = index.Entries;
    }
}

public class Entry(string tag, int indexOuter, int indexInner, string fileOuter, string fileInner)
{
    public string Tag { get; set; } = tag;
    public int IndexOuter { get; set; } = indexOuter;
    public int IndexInner { get; set; } = indexInner;
    public string FileOuter { get; set; } = fileOuter;
    public string FileInner { get; set; } = fileInner;
}