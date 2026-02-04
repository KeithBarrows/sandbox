using Microsoft.Extensions.Logging;

namespace OneMinuteChallenge;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();


#if DEBUG
	builder.Services.AddBlazorWebViewDeveloperTools();
	builder.Logging.AddDebug();
#endif

	// Register storage service
	builder.Services.AddSingleton<Services.JsonStorageService>();

	return builder.Build();
    }
}
