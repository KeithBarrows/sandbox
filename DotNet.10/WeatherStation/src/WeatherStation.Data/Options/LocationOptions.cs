namespace WeatherStation.Data.Options;

public class LocationOptions
{
    // Short, stable identifier stored on every row so multiple locations can share the
    // same tables -- must never change once data has been collected under it.
    public string Key { get; set; } = "";

    public string DisplayName { get; set; } = "";

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string TimeZone { get; set; } = "America/New_York";

    // The dashboard's full metric set (current conditions, all charts) renders for
    // whichever location has this set; every other configured location is treated as
    // secondary and only overlays its pressure onto the primary's pressure chart.
    public bool IsPrimary { get; set; }
}
