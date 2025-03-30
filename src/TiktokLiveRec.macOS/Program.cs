using Avalonia;
using Avalonia.Dialogs;
using Avalonia.Media;
using System.Runtime.Versioning;

namespace TiktokLiveRec.macOS;

[SupportedOSPlatform("Windows")]
[SupportedOSPlatform("macOS")]
[SupportedOSPlatform("linux")]
internal sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .With(new FontManagerOptions
        {
            FontFallbacks =
            [
                new FontFallback
                {
                    FontFamily = new FontFamily("Microsoft YaHei")
                }
            ]
        })
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UseManagedSystemDialogs()
            .UsePlatformDetect()
            .With(new Win32PlatformOptions())
            .WithInterFont()
            .LogToTrace();
}
