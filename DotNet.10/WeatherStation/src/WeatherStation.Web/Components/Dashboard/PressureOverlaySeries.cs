namespace WeatherStation.Web.Components.Dashboard;

// A secondary location's pressure-only data point, converted to that location's own
// local time (each location keeps its own time zone, unlike the primary ChartRow axis).
public sealed record PressureOverlayPoint(DateTime LocalTime, decimal? MinInHg, decimal? MaxInHg, decimal? AvgInHg);

// One secondary location overlaid onto the primary location's pressure chart. Min/Max
// render as marker points rather than a shaded band (like the primary series gets) --
// multiple overlapping bands would be unreadable, but a couple of dots per bucket reads
// fine alongside the primary's band.
public sealed record PressureOverlaySeries(string Name, string Color, IReadOnlyList<PressureOverlayPoint> Points);
