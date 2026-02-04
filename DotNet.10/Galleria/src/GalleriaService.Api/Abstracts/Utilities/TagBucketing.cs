using System;
using System.Collections.Generic;
using System.Linq;
using GalleriaService.Api.Abstracts.Models;

namespace GalleriaService.Api.Abstracts.Utilities;

public static class TagBucketing
{
    public static List<DtoTagInfo> AssignBuckets(IEnumerable<DtoTagInfo> tags, int numberOfBuckets)
    {
        var results = tags.ToList();

        if (!results.Any()) return results;

        // Find min and max counts
        int minCount = results.Min(t => t.Count);
        int maxCount = results.Max(t => t.Count);

        // If all counts are the same, assign to middle bucket
        if (minCount == maxCount)
        {
            results.ForEach(t => t.Bucket = numberOfBuckets / 2);
            return results;
        }

        // Calculate the range for each bucket
        double range = (maxCount - minCount) / (double)numberOfBuckets;

        // Assign buckets based on count ranges
        foreach (var tag in results)
        {
            // Use floor to get bucket number (0-based)
            int bucket = (int)Math.Floor((tag.Count - minCount) / range);

            // Handle edge case where count equals maxCount
            tag.Bucket = Math.Min(bucket, numberOfBuckets - 1);
        }

        return results;
    }
}

/*
 * Example usage:
 *

var tags = new List<TagCount>
{
    new() { Tag = "nature", Count = 100 },
    new() { Tag = "portrait", Count = 50 },
    new() { Tag = "landscape", Count = 75 },
    new() { Tag = "wildlife", Count = 25 },
    new() { Tag = "macro", Count = 10 }
};

// Distribute into 5 buckets (0-4)
var bucketed = TagBucketing.AssignBuckets(tags, 5);

foreach (var tag in bucketed)
{
    Console.WriteLine($"{tag.Tag}: Count={tag.Count}, Bucket={tag.Bucket}");
}

 *
 *
 */