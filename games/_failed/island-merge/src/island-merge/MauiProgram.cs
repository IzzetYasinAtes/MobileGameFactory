using IslandMerge.Services;
using IslandMerge.ViewModels;
using IslandMerge.Views;
using Microsoft.Extensions.DependencyInjection;
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

        // Services
        builder.Services.AddSingleton<IStorage, SqliteStorage>();
        builder.Services.AddSingleton<IAudio, NullAudio>();
        builder.Services.AddSingleton<IGameSession, GameSession>();
        builder.Services.AddSingleton<IInterstitialGuard, InterstitialGuard>();
        builder.Services.AddSingleton<IRewardedCooldown, RewardedCooldown>();
        builder.Services.AddSingleton<ISelectedCharacterStore, SelectedCharacterStore>();

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
        builder.Services.AddTransient<CharacterSelectViewModel>();

        // Pages
        builder.Services.AddTransient<MainMenuPage>();
        builder.Services.AddTransient<BoardPage>();
        builder.Services.AddTransient<BiomeSelectPage>();
        builder.Services.AddTransient<ShopPage>();
        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<CharacterSelectPage>();

        var app = builder.Build();

        // Initialize session on startup (one-shot, non-blocking for UI).
        _ = Task.Run(async () =>
        {
            try
            {
                var guard = app.Services.GetService<IInterstitialGuard>();
                guard?.NotifySessionStarted();
            }
            catch
            {
                // ignore
            }
        });

        return app;
    }
}
