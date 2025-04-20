using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace FluentAvalonia.UI.Controls;

public static class MessageBox
{
    public static async Task<MessageBoxResult> InfoAsync(string messageBoxText)
        => await ShowAsync(GetActiveWindow(), messageBoxText, null!, MessageBoxButton.OK, MessageBoxImage.Information);

    public static async Task<MessageBoxResult> InfoAsync(string messageBoxText, string caption)
        => await ShowAsync(GetActiveWindow(), messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Information);

    public static async Task<MessageBoxResult> InfoAsync(string messageBoxText, string caption, MessageBoxButton button)
        => await ShowAsync(GetActiveWindow(), messageBoxText, caption, button, MessageBoxImage.Information);

    public static async Task<MessageBoxResult> InfoAsync(Window? owner, string messageBoxText)
        => await ShowAsync(owner ?? GetActiveWindow(), messageBoxText, null!, MessageBoxButton.OK, MessageBoxImage.Information);

    public static async Task<MessageBoxResult> InfoAsync(Window? owner, string messageBoxText, string caption)
        => await ShowAsync(owner ?? GetActiveWindow(), messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Information);

    public static async Task<MessageBoxResult> InfoAsync(Window? owner, string messageBoxText, string caption, MessageBoxButton button)
        => await ShowAsync(owner ?? GetActiveWindow(), messageBoxText, caption, button, MessageBoxImage.Information);

    public static async Task<MessageBoxResult> WarnAsync(string messageBoxText)
        => await ShowAsync(GetActiveWindow(), messageBoxText, null!, MessageBoxButton.OK, MessageBoxImage.Warning);

    public static async Task<MessageBoxResult> WarnAsync(string messageBoxText, string caption)
        => await ShowAsync(GetActiveWindow(), messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Warning);

    public static async Task<MessageBoxResult> WarnAsync(string messageBoxText, string caption, MessageBoxButton button)
        => await ShowAsync(GetActiveWindow(), messageBoxText, caption, button, MessageBoxImage.Warning);

    public static async Task<MessageBoxResult> WarnAsync(Window? owner, string messageBoxText)
        => await ShowAsync(owner ?? GetActiveWindow(), messageBoxText, null!, MessageBoxButton.OK, MessageBoxImage.Warning);

    public static async Task<MessageBoxResult> WarnAsync(Window? owner, string messageBoxText, string caption)
        => await ShowAsync(owner ?? GetActiveWindow(), messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Warning);

    public static async Task<MessageBoxResult> WarnAsync(Window? owner, string messageBoxText, string caption, MessageBoxButton button)
        => await ShowAsync(owner ?? GetActiveWindow(), messageBoxText, caption, button, MessageBoxImage.Warning);

    public static async Task<MessageBoxResult> ErrorAsync(string messageBoxText)
        => await ShowAsync(GetActiveWindow(), messageBoxText, null!, MessageBoxButton.OK, MessageBoxImage.Error);

    public static async Task<MessageBoxResult> ErrorAsync(string messageBoxText, string caption)
        => await ShowAsync(GetActiveWindow(), messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Error);

    public static async Task<MessageBoxResult> ErrorAsync(string messageBoxText, string caption, MessageBoxButton button)
        => await ShowAsync(GetActiveWindow(), messageBoxText, caption, button, MessageBoxImage.Error);

    public static async Task<MessageBoxResult> ErrorAsync(Window? owner, string messageBoxText)
        => await ShowAsync(owner ?? GetActiveWindow(), messageBoxText, null!, MessageBoxButton.OK, MessageBoxImage.Error);

    public static async Task<MessageBoxResult> ErrorAsync(Window? owner, string messageBoxText, string caption)
        => await ShowAsync(owner ?? GetActiveWindow(), messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Error);

    public static async Task<MessageBoxResult> ErrorAsync(Window? owner, string messageBoxText, string caption, MessageBoxButton button)
        => await ShowAsync(owner ?? GetActiveWindow(), messageBoxText, caption, button, MessageBoxImage.Error);

    public static async Task<MessageBoxResult> QuestionAsync(string messageBoxText)
        => await ShowAsync(GetActiveWindow(), messageBoxText, null!, MessageBoxButton.YesNo, MessageBoxImage.Question);

    public static async Task<MessageBoxResult> QuestionAsync(string messageBoxText, string caption)
        => await ShowAsync(GetActiveWindow(), messageBoxText, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);

    public static async Task<MessageBoxResult> QuestionAsync(string messageBoxText, string caption, MessageBoxButton button)
        => await ShowAsync(GetActiveWindow(), messageBoxText, caption, button, MessageBoxImage.Question);

    public static async Task<MessageBoxResult> QuestionAsync(Window? owner, string messageBoxText)
        => await ShowAsync(owner ?? GetActiveWindow(), messageBoxText, null!, MessageBoxButton.YesNo, MessageBoxImage.Question);

    public static async Task<MessageBoxResult> QuestionAsync(Window? owner, string messageBoxText, string caption)
        => await ShowAsync(owner ?? GetActiveWindow(), messageBoxText, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);

    public static async Task<MessageBoxResult> QuestionAsync(Window? owner, string messageBoxText, string caption, MessageBoxButton button)
        => await ShowAsync(owner ?? GetActiveWindow(), messageBoxText, caption, button, MessageBoxImage.Question);

    public static async Task<MessageBoxResult> ShowAsync(string messageBoxText)
        => await ShowAsync(GetActiveWindow(), messageBoxText, null!, MessageBoxButton.OK, MessageBoxImage.Information);

    public static async Task<MessageBoxResult> ShowAsync(string messageBoxText, string caption)
        => await ShowAsync(GetActiveWindow(), messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Information);

    public static async Task<MessageBoxResult> ShowAsync(string messageBoxText, string caption, MessageBoxButton button)
        => await ShowAsync(GetActiveWindow(), messageBoxText, caption, button, MessageBoxImage.Information);

    public static async Task<MessageBoxResult> ShowAsync(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        => await ShowAsync(GetActiveWindow(), messageBoxText, caption, button, icon);

    public static async Task<MessageBoxResult> ShowAsync(Window? owner, string messageBoxText)
        => await ShowAsync(owner ?? GetActiveWindow(), messageBoxText, null!, MessageBoxButton.OK, MessageBoxImage.Information);

    public static async Task<MessageBoxResult> ShowAsync(Window? owner, string messageBoxText, string caption)
        => await ShowAsync(owner ?? GetActiveWindow(), messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Information);

    public static async Task<MessageBoxResult> ShowAsync(Window? owner, string messageBoxText, string caption, MessageBoxButton button)
        => await ShowAsync(owner ?? GetActiveWindow(), messageBoxText, caption, button, MessageBoxImage.Information);

    public static async Task<MessageBoxResult> ShowAsync(Window? owner, string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
    {
        owner ??= GetActiveWindow();

        MessageBoxWindow messageBox = new(
            new MessageBoxStandardParams
            {
                ButtonDefinitions = button,
                ContentTitle = caption,
                ContentMessage = messageBoxText,
                Icon = icon,
                WindowIcon = owner?.Icon!,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                CanResize = false,
                MaxWidth = 500,
                MinWidth = 120,
                MaxHeight = 800,
                SizeToContent = SizeToContent.WidthAndHeight,
                ShowInCenter = true,
                Topmost = owner?.Topmost ?? false,
            });

        await messageBox.ShowDialog(owner!);
        return messageBox.Result;
    }

    private static Window? GetActiveWindow()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            return desktop.Windows.FirstOrDefault(window => window.IsActive && window.ShowActivated) ?? desktop.MainWindow;
        }

        return null;
    }
}
