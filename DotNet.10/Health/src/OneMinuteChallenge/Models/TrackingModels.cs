using System;
using System.Collections.Generic;

namespace OneMinuteChallenge.Models
{
    public class Meal
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class GlucoseReading
    {
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
    }

    public class TrackingEntry
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string MealId { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public double StartGlucose { get; set; }
        public DateTime? EndTime { get; set; }
        public double? EndGlucose { get; set; }
        public List<GlucoseReading> IntervalReadings { get; set; } = new();
        public string? HowYouFeel { get; set; }
    }
}
