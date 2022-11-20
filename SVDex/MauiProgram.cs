using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace SVDex;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder().UseMauiCommunityToolkit(); ;
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
