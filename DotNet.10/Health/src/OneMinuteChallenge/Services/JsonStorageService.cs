using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using OneMinuteChallenge.Models;


namespace OneMinuteChallenge.Services
{
    public class JsonStorageService
    {
        private readonly string _basePath;
        public JsonStorageService()
        {
#if WINDOWS
            _basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
#else
            _basePath = FileSystem.Current.AppDataDirectory;
#endif
        }

        private string MealsFile => Path.Combine(_basePath, "meals.json");
        private string TrackingFile => Path.Combine(_basePath, "tracking.json");

        public async Task<List<Meal>> GetMealsAsync()
        {
            if (!File.Exists(MealsFile)) return new List<Meal>();
            var json = await File.ReadAllTextAsync(MealsFile);
            return JsonSerializer.Deserialize<List<Meal>>(json) ?? new();
        }

        public async Task SaveMealsAsync(List<Meal> meals)
        {
            var json = JsonSerializer.Serialize(meals, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(MealsFile, json);
        }

        public async Task<List<TrackingEntry>> GetTrackingEntriesAsync()
        {
            if (!File.Exists(TrackingFile)) return new List<TrackingEntry>();
            var json = await File.ReadAllTextAsync(TrackingFile);
            return JsonSerializer.Deserialize<List<TrackingEntry>>(json) ?? new();
        }

        public async Task SaveTrackingEntriesAsync(List<TrackingEntry> entries)
        {
            var json = JsonSerializer.Serialize(entries, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(TrackingFile, json);
        }
    }
}
