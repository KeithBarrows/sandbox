namespace WeatherStation.Web.Services;

// Singleton pub/sub so the Dashboard can refresh the instant the collector saves a new
// observation, instead of waiting for its periodic poll timer to catch up.
public sealed class WeatherUpdateNotifier
{
    public event Action? Updated;

    public void NotifyUpdated() => Updated?.Invoke();
}
