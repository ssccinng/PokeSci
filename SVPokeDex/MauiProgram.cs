using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using SVPokeDex.ViewModel;
using SVPokeDex.View;

namespace SVPokeDex;
public static class MauiProgram
{

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        }).UseMauiCommunityToolkit();

        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddTransient<MainPage>();

        builder.Services.AddSingleton<SettingViewModel>();
        builder.Services.AddTransient<SettingPage>();


        builder.Services.AddSingleton<PokeDexViewModel>();
        builder.Services.AddTransient<PokeDexPage>();

        //builder.Services.AddSingleton<ShellViewModel>();
        //builder.Services.AddTransient<AppShell>();



#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}