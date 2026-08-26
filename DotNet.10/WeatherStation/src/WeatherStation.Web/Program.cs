using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Radzen;
using Serilog;
using WeatherStation.Data;
using WeatherStation.Data.Options;
using WeatherStation.Data.Querying;
using WeatherStation.Web.Components;
using WeatherStation.Web.Services;

// Bootstrap logger covers startup itself (config binding, DI container build) before
// the full Serilog pipeline below is wired up from appsettings.json.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting WeatherStation.Web...");

    var builder = WebApplication.CreateBuilder(args);

    // No-op unless actually launched by the Windows Service Control Manager; sets the
    // content root to the executable's directory so appsettings.json is found regardless
    // of the service's working directory, and hooks up SCM start/stop signaling.
    builder.Host.UseWindowsService(options => options.ServiceName = "WeatherStation");

    // Console logging goes nowhere once running as a service (no attached console), so
    // Serilog's File and Seq sinks (configured in appsettings.json) are what actually
    // carry logs -- otherwise background-service failures are silently invisible.
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    // Add services to the container.
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    builder.Services.AddRadzenComponents();

    builder.Services.Configure<List<LocationOptions>>(builder.Configuration.GetSection("Locations"));
    builder.Services.Configure<CollectorOptions>(builder.Configuration.GetSection("Collector"));

    builder.Services.AddDbContext<WeatherDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("WeatherDb")));
    builder.Services.AddScoped<IWeatherQueryService, WeatherQueryService>();
    builder.Services.AddSingleton<WeatherUpdateNotifier>();

    // Polls only happen every 15 min, so a pooled connection sitting idle that long is
    // often already dead (killed by a NAT/firewall timeout) by the time it's reused --
    // that shows up as "connection forcibly closed by remote host" on an otherwise healthy
    // network. Recycling the pool every couple of minutes forces a fresh connection well
    // before that can happen.
    static SocketsHttpHandler CreateHandler() => new() { PooledConnectionLifetime = TimeSpan.FromMinutes(2) };

    builder.Services.AddHttpClient<OpenMeteoClient>(client =>
    {
        client.BaseAddress = new Uri("https://api.open-meteo.com/");
        client.Timeout = TimeSpan.FromSeconds(15);
    }).ConfigurePrimaryHttpMessageHandler(CreateHandler);
    builder.Services.AddHttpClient<OpenMeteoArchiveClient>(client =>
    {
        client.BaseAddress = new Uri("https://archive-api.open-meteo.com/");
        client.Timeout = TimeSpan.FromSeconds(30);
    }).ConfigurePrimaryHttpMessageHandler(CreateHandler);

    builder.Services.AddHostedService<HistoricalBackfillService>();
    builder.Services.AddHostedService<WeatherCollectorService>();

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();
        db.Database.Migrate();
    }

    // Configure the HTTP request pipeline. Plain HTTP only (no HTTPS redirect/HSTS) --
    // this runs on the home LAN behind no reverse proxy, so there's no cert to redirect to.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
    }

    app.UseAntiforgery();

    app.MapStaticAssets();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    // HostAbortedException is thrown deliberately by EF Core's design-time tooling
    // (e.g. `dotnet ef migrations add`) after building the host but before running it --
    // letting it propagate is what allows that tooling to work.
    Log.Fatal(ex, "WeatherStation.Web terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}
