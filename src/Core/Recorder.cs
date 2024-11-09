using CommunityToolkit.Mvvm.Messaging;
using Flucli;
using System.Diagnostics;
using System.IO;
using System.Text;
using TiktokLiveRec.Extensions;
using TiktokLiveRec.Models;

namespace TiktokLiveRec.Core;

public sealed class Recorder
{
    public RecordStatus RecordStatus { get; private set; } = RecordStatus.Initialized;

    public Process? RecorderProcess { get; private set; } = null;

    public CancellationTokenSource? TokenSource { get; private set; } = null;

    public string? FileName { get; set; } = null;

    public string? Parameters { get; set; } = null;

    public Task Start(RecorderStartInfo startInfo, CancellationTokenSource? tokenSource = null)
    {
        if (RecordStatus == RecordStatus.Recording)
        {
            // Already recording
            return Task.CompletedTask;
        }

        RecordStatus = RecordStatus.Recording;
        // Start a recording task that does not use the default ThreadPool
        return Task.Factory.StartNew(async () =>
        {
            try
            {
                string? recorderPath = SearchFileHelper.SearchFiles(".", "ffmpeg.exe").FirstOrDefault();

                if (recorderPath == null)
                {
                    // Error on Recorder not found
                    RecordStatus = RecordStatus.Error;
                    return;
                }

                string saveFolder = SaveFolderHelper.GetSaveFolder(Configurations.SaveFolder.Get());

                saveFolder = Path.Combine(saveFolder, startInfo.NickName);
                if (!Directory.Exists(saveFolder))
                {
                    Directory.CreateDirectory(saveFolder);
                }

                FileName = Path.Combine(saveFolder, $"{startInfo.NickName}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.ts");
                Parameters = $"-i \"{startInfo.HlsUrl}\" -c copy \"{FileName}\" -hide_banner -y -user_agent \"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36 Edg/130.0.0.0\"";

                TokenSource = tokenSource ?? new CancellationTokenSource();

                await recorderPath
                    .WithArguments(Parameters)
                    .WithStandardErrorPipe(PipeTarget.ToDelegate(OnStandardErrorReceived, Encoding.UTF8))
                    .WithStandardOutputPipe(PipeTarget.ToDelegate(OnStandardOutputReceived, Encoding.UTF8))
                    .ExecuteAsync(cancellationToken: TokenSource.Token);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            RecordStatus = RecordStatus.NotRecording;
        }, TaskCreationOptions.LongRunning);
    }

    public void Stop()
    {
        TokenSource?.Cancel();
    }

    private Task OnStandardErrorReceived(string data, CancellationToken token)
    {
        Debug.WriteLine(data);
        _ = WeakReferenceMessenger.Default.Send(new RecorderMessage()
        {
            DataType = StandardData.StandardError,
            Data = data,
        });
        return Task.CompletedTask;
    }

    private Task OnStandardOutputReceived(string data, CancellationToken token)
    {
        Debug.WriteLine(data);
        _ = WeakReferenceMessenger.Default.Send(new RecorderMessage()
        {
            DataType = StandardData.StandardOutput,
            Data = data,
        });
        return Task.CompletedTask;
    }
}

public enum RecordStatus
{
    Initialized,
    Disabled,
    NotRecording,
    Recording,
    Error,
}

public record RecorderStartInfo
{
    public string NickName { get; set; } = string.Empty;

    public string HlsUrl { get; set; } = string.Empty;
}
