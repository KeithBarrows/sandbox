using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace WeatherApp.Api;

public class WeatherMetrics
{
    public const string MeterName = "WeatherApp.Api";

    private readonly Counter<long> _weatherRequestCounter;
    private readonly Histogram<double> _weatherRequestDuration;
    
    public WeatherMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);
        _weatherRequestCounter = meter.CreateCounter<long>(
            "weatherapp.api.weather_requests.count");

        _weatherRequestDuration = meter.CreateHistogram<double>(
            "weatherapp.api.weather_requests.duration",
            "ms");
    }

    public void IncreaseWeatherRequestCount()
    {
        _weatherRequestCounter.Add(1);
    }

    public TrackedRequestDuration MeasureRequestDuration()
    {
        return new TrackedRequestDuration(_weatherRequestDuration);
    }
}

public class TrackedRequestDuration : IDisposable
{
    private readonly long _requestStartTime = TimeProvider.System.GetTimestamp();
    private readonly Histogram<double> _histogram;

    public TrackedRequestDuration(Histogram<double> histogram)
    {
        _histogram = histogram;
    }

    public void Dispose()
    {
        var elapsed = TimeProvider.System.GetElapsedTime(_requestStartTime);
        _histogram.Record(elapsed.TotalMilliseconds);
    }
}
