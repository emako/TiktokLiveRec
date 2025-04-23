using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Uwp.Notifications;
using NAudio.Wave;
using System.Net;
using System.Net.Mail;

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

    public static void AddNoticeWithButton(string header, string title, ToastContentButtonOption[] buttons, ToastDuration duration = ToastDuration.Short)
    {
        if (Environment.OSVersion.Version.Major < 10)
        {
            return;
        }

        new ToastContentBuilder()
            .AddHeader("AddNotice", header, "AddNotice")
            .AddText(title)
            .AddButtons(buttons)
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

    public static async Task PlayMusicAsync(Stream stream)
    {
        using MusicPlayer player = new(stream);
        player.Play();

        CancellationTokenSource source = new();
        _ = Task.Run(async () =>
        {
            await Task.Delay(30000);
            if (source.Token.CanBeCanceled)
            {
                source.Cancel();
            }
        });
        await player.WaitAsync(source.Token);
    }

    public static bool SendEmail(string smtpServer, string userName, string password, string nickName, string roomUrl)
    {
        try
        {
            using MailMessage mail = new();
            mail.From = new MailAddress(userName);
            mail.To.Add(userName);
            mail.Subject = $"{nickName}{Locale.Culture.WordSpace()}{"LiveNotification".Tr()} - TiktokLiveRec";
            mail.Body = $"<html><body>{"MailBodyElement".Tr(nickName)} <a href=\"{roomUrl}\">{roomUrl}</a></body></html>";
            mail.IsBodyHtml = true;

            using SmtpClient smtp = new(smtpServer, 25);
            smtp.Credentials = new NetworkCredential(userName, password);
            smtp.EnableSsl = true;
            smtp.Send(mail);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error sending email: " + ex.Message);
        }
        return false;
    }
}

internal sealed class ToastNotificationActivatedMessage(ToastNotificationActivatedEventArgsCompat e)
{
    public ToastNotificationActivatedEventArgsCompat EventArgs { get; } = e;
}

public sealed class ToastContentButtonOption
{
    public string Content { get; set; } = string.Empty;

    public (string key, string value)[] Arguments { get; set; } = [];

    public ToastActivationType ActivationType { get; set; } = ToastActivationType.Background;
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

    public static ToastContentBuilder AddButtons(this ToastContentBuilder builder, params ToastContentButtonOption[] buttonOptions)
    {
        foreach (ToastContentButtonOption buttonOption in buttonOptions)
        {
            ToastButton button = new ToastButton()
                .SetContent(buttonOption.Content)
                .AddArguments(buttonOption.Arguments);

            button.ActivationType = buttonOption.ActivationType;
            builder.AddButton(button);
        }
        return builder;
    }
}

file static class ToastButtonExtensions
{
    public static ToastButton AddArguments(this ToastButton toastButton, (string, string)[] args)
    {
        foreach ((string, string) arg in args)
        {
            toastButton.AddArgument(arg.Item1, arg.Item2);
        }
        return toastButton;
    }
}

file sealed partial class MusicPlayer : IDisposable
{
    private bool disposed = false;
    private readonly Mp3FileReader mp3FileReader;
    private readonly WaveOut waveOut;

    public MusicPlayer(Stream stream)
    {
        mp3FileReader = new Mp3FileReader(stream);
        waveOut = new WaveOut();
        waveOut.Init(mp3FileReader);
    }

    public void Play()
    {
        waveOut.Play();
    }

    public void Stop()
    {
        waveOut.Stop();
        mp3FileReader.Position = 0;
    }

    public void Pause()
    {
        waveOut.Stop();
    }

    public async Task WaitAsync(CancellationToken token = default)
    {
        await Task.Run(() =>
        {
            try
            {
                SpinWait.SpinUntil(() =>
                {
                    if (token.IsCancellationRequested)
                    {
                        Stop();
                        token.ThrowIfCancellationRequested();
                    }

                    return waveOut.PlaybackState != PlaybackState.Playing;
                });
            }
            catch (OperationCanceledException)
            {
                ///
            }
        }, token);
    }

    public void Closed()
    {
        Dispose();
    }

    public void Dispose()
    {
        CleanUp(true);
        GC.SuppressFinalize(this);
    }

    private void CleanUp(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                waveOut.Dispose();
                mp3FileReader.Dispose();
            }
        }
        disposed = true;
    }
}
