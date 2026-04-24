using IslandMerge.Services;
using Microsoft.Extensions.Logging;

namespace IslandMerge;

public partial class App : Application
{
    private readonly IServiceProvider _services;
    private readonly ILogger<App> _logger;

    public App(IServiceProvider services, ILogger<App> logger)
    {
        InitializeComponent();
        _services = services;
        _logger = logger;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell());
        window.Deactivated += OnWindowDeactivated;
        window.Destroying += OnWindowDestroying;
        window.Stopped += OnWindowStopped;
        return window;
    }

    private void OnWindowDeactivated(object? sender, EventArgs e) => FlushSessionFireAndForget("deactivated");

    private void OnWindowStopped(object? sender, EventArgs e) => FlushSessionFireAndForget("stopped");

    private void OnWindowDestroying(object? sender, EventArgs e) => FlushSessionFireAndForget("destroying");

    private void FlushSessionFireAndForget(string reason)
    {
        // Fire-and-forget: lifecycle event'leri senkron sinyal bekler; blocking wait yapmayalim.
        _ = Task.Run(async () =>
        {
            try
            {
                var session = _services.GetService(typeof(IGameSession)) as IGameSession;
                if (session is null)
                {
                    return;
                }
                await session.FlushAsync().ConfigureAwait(false);
                _logger.LogDebug("Session flushed on {Reason}", reason);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Flush on {Reason} failed", reason);
            }
        });
    }
}
