namespace TiktokLiveRec;

internal class AppConfig
{
    public static string PackName => "TiktokLiveRec";
    public static string Version => $"v{typeof(App).Assembly.GetName().Version!.ToString(3)}";
    public static string Url => "https://github.com/emako/TiktokLiveRec";
}
