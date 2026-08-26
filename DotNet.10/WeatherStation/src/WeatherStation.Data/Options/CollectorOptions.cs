namespace WeatherStation.Data.Options;

public class CollectorOptions
{
    public int PollIntervalMinutes { get; set; } = 15;
    public int BackfillDays { get; set; } = 365;
}
