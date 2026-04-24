using Android.App;
using Android.Content;
using Android.Runtime;
using IslandMerge.Services;

namespace IslandMerge;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    /// <summary>
    /// P1-005: OS bellek baskisi altinda state flush. TrimMemoryRunningModerate (5) ve uzerinde persist.
    /// </summary>
    public override void OnTrimMemory(TrimMemory level)
    {
        base.OnTrimMemory(level);
        if ((int)level < (int)TrimMemory.RunningModerate)
        {
            return;
        }

        // Fire-and-forget: lifecycle event senkron sinyal; block etme.
        _ = Task.Run(async () =>
        {
            try
            {
                var services = Microsoft.Maui.IPlatformApplication.Current?.Services;
                if (services?.GetService(typeof(IGameSession)) is IGameSession session)
                {
                    await session.FlushAsync().ConfigureAwait(false);
                }
            }
            catch
            {
                // Silent: lifecycle fire-and-forget.
            }
        });
    }
}
