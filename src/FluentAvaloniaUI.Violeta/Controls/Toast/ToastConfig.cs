using Avalonia;

namespace FluentAvalonia.UI.Controls;

public class ToastConfig
{
    public const int FastTime = 1500;
    public const int NormalTime = 2000;
    public const int SlowTime = 3000;

    public ToastIcon Icon { get; set; } = ToastIcon.Information;
    public int Time { get; set; } = NormalTime;
    public ToastLocation Location { get; set; } = ToastLocation.TopCenter;
    public Thickness Margin { get; set; } = new Thickness(default);

    public ToastConfig(ToastIcon icon, ToastLocation location, int time = FastTime) : this()
    {
        Icon = icon;
        Location = location;
        Time = time;
    }

    public ToastConfig()
    {
    }
}

public enum ToastIcon
{
    None,
    Information,
    Success,
    Error,
    Warning,
    Question,
}

public enum ToastLocation
{
    Center,
    Left,
    Right,
    TopLeft,
    TopCenter,
    TopRight,
    BottomLeft,
    BottomCenter,
    BottomRight,
}
