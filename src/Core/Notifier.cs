using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Uwp.Notifications;

namespace TiktokLiveRec.Core;

internal static class Notifier
{
    static Notifier()
    {
        ToastNotificationManagerCompat.OnActivated += (ToastNotificationActivatedEventArgsCompat e) =>
        {
            WeakReferenceMessenger.Default.Send(new ToastNotificationActivatedMessage(e));
        };
    }

    public static void AddNotice(string header, string title, string detail = null!, ToastDuration duration = ToastDuration.Short)
    {
        if (Environment.OSVersion.Version.Major < 10)
        {
            return;
        }

        new ToastContentBuilder()
            .AddHeader("AddNotice", header, "AddNotice")
            .AddText(title)
            .AddAttributionTextIf(!string.IsNullOrEmpty(detail), detail)
            .SetToastDuration(duration)
            .Show();
    }

    public static void AddNoticeWithButton(string header, string title, string button, (string, string)[] args, ToastDuration duration = ToastDuration.Short)
    {
        if (Environment.OSVersion.Version.Major < 10)
        {
            return;
        }

        new ToastContentBuilder()
            .AddHeader("AddNotice", header, "AddNotice")
            .AddText(title)
            .AddButton(
                new ToastButton()
                    .SetContent(button)
                    .AddArguments(args)
                    .SetBackgroundActivation()
            )
            .SetToastDuration(duration)
            .Show();
    }

    public static void ClearNotice()
    {
        if (Environment.OSVersion.Version.Major < 10)
        {
            return;
        }

        try
        {
            ToastNotificationManagerCompat.History.Clear();
        }
        catch
        {
        }
    }
}

internal sealed class ToastNotificationActivatedMessage(ToastNotificationActivatedEventArgsCompat e)
{
    public ToastNotificationActivatedEventArgsCompat EventArgs { get; } = e;
}

file static class ToastContentBuilderExtensions
{
    public static ToastContentBuilder AddAttributionTextIf(this ToastContentBuilder builder, bool condition, string text)
    {
        if (condition)
        {
            return builder.AddAttributionText(text);
        }
        else
        {
            return builder;
        }
    }
}

file static class ToastButtonExtensions
{
    public static ToastButton AddArguments(this ToastButton toastButton, (string, string)[] args)
    {
        foreach (var arg in args)
        {
            toastButton.AddArgument(arg.Item1, arg.Item2);
        }
        return toastButton;
    }
}