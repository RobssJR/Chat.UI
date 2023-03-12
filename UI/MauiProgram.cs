namespace UI;

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
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

				fonts.AddFont("Brands-Regular.otf", "FAB");
				fonts.AddFont("Free-Regular.otf", "FAR");
				fonts.AddFont("Free-Solid.otf", "FAS");
			});

        return builder.Build();
	}
}
