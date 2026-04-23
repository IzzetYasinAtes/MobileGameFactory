using IslandMerge.Services;
using IslandMerge.ViewModels;
using IslandMerge.Views;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace IslandMerge;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseSkiaSharp()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Services (singleton — oturum genisliginde state)
        builder.Services.AddSingleton<IStorage, SqliteStorage>();
        builder.Services.AddSingleton<IAudio, NullAudio>();
        builder.Services.AddSingleton<IGameSession, GameSession>();

#if ANDROID
        builder.Services.AddSingleton<IAdService, IslandMerge.Platforms.Android.Services.AdMobAdService>();
        builder.Services.AddSingleton<IIapService, IslandMerge.Platforms.Android.Services.PlayBillingIapService>();
#else
        builder.Services.AddSingleton<IAdService, StubAdService>();
        builder.Services.AddSingleton<IIapService, StubIapService>();
#endif

        // ViewModels
        builder.Services.AddTransient<MainMenuViewModel>();
        builder.Services.AddTransient<BoardViewModel>();
        builder.Services.AddTransient<BiomeSelectViewModel>();
        builder.Services.AddTransient<ShopViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();

        // Pages
        builder.Services.AddTransient<MainMenuPage>();
        builder.Services.AddTransient<BoardPage>();
        builder.Services.AddTransient<BiomeSelectPage>();
        builder.Services.AddTransient<ShopPage>();
        builder.Services.AddTransient<SettingsPage>();

        return builder.Build();
    }
}
