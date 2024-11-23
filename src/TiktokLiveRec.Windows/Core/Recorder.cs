using CommunityToolkit.Mvvm.Messaging;
using Flucli;
using Flucli.Utils.Extensions;
using System.Diagnostics;
using System.IO;
using System.Text;
using TiktokLiveRec.Extensions;
using TiktokLiveRec.Models;

namespace TiktokLiveRec.Core;

public sealed class Recorder
{
    public RecordStatus RecordStatus { get; internal set; } = RecordStatus.Initialized;

    public CancellationTokenSource? TokenSource { get; private set; } = null;

    public string? FileName { get; set; } = null;

    public string? Parameters { get; set; } = null;

    public DateTime StartTime { get; private set; } = DateTime.MinValue;

    public DateTime EndTime { get; private set; } = DateTime.MinValue;

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
                string? recorderPath = SearchFileHelper.SearchFiles(".", "ffmpeg[\\.exe]").FirstOrDefault();

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

                string userAgent = Configurations.UserAgent.Get();
                string httpProxy = Configurations.ProxyUrl.Get();
                bool isUseProxy = Configurations.IsUseProxy.Get() && !string.IsNullOrWhiteSpace(httpProxy);

                if (string.IsNullOrWhiteSpace(userAgent))
                {
                    userAgent = "Mozilla/5.0 (Linux; Android 11; SAMSUNG SM-G973U) AppleWebKit/537.36 ("
                              + "KHTML, like Gecko) SamsungBrowser/14.2 Chrome/87.0.4280.141 Mobile "
                              + "Safari/537.36";
                }

                FileName = Path.Combine(saveFolder, $"{startInfo.NickName}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.ts");
                Parameters = new List<string>() {
                    "-y",                             // Overwrite output files.
                    "-v", "verbose",                  // Set logging level to `verbose`.
                    "-rw_timeout", "15000000",        // Set maximum time to wait for (network) read/write operations to complete, in microseconds to `15 seconds`.
                    "-loglevel", "error",             // Set logging level to `error`.
                    "-hide_banner",                   // Suppress printing banner.
                    "-user_agent", userAgent,         // Override the User-Agent header. If not specified the protocol will use a string describing the libavformat build. ("Lavf/<version>")
                    "-protocol_whitelist", "rtmp,crypto,file,http,https,tcp,tls,udp,rtp,httpproxy",
                                                      // Set a ","-separated list of allowed protocols.
                                                      // "ALL" matches all protocols.
                                                      // Protocols prefixed by "-" are disabled.
                                                      // All protocols are allowed by default but protocols used by an another protocol (nested protocols) are restricted to a per protocol subset.
                    "-thread_queue_size", "1024",     // For input, this option sets the maximum number of queued packets when reading from the file or device.
                                                      // With low latency / high rate live streams, packets may be discarded if they are not read in a timely manner; setting this value can force ffmpeg to use a separate input thread and read packets as soon as they arrive.
                                                      // By default ffmpeg only does this if multiple inputs are specified.
                                                      // For output, this option specified the maximum number of packets that may be queued to each muxing thread.
                    "-analyzeduration", "20000000",   // Specify how many microseconds are analyzed to probe the input.
                                                      // A higher value will enable detecting more accurate information, but will increase latency.
                                                      // It defaults to 5,000,000 microseconds = 5 seconds.
                                                      // Set to 20,000,000 microseconds = 20 seconds.
                    "-probesize", "10000000",         // Set probing size in bytes, i.e. the size of the data to analyze to get stream information.
                                                      // A higher value will enable detecting more information in case it is dispersed into the stream, but will increase latency.
                                                      // Must be an integer not lesser than 32. It is 5000000 by default.
                    "-fflags", "+discardcorrupt",     // Set format flags. Some are implemented for a limited number of formats.
                                                      // Set to +discardcorrupt: Discard corrupted packets.
                    "-i", startInfo.HlsUrl,           // Input infile.
                    "-bufsize", "8000k",              // Specifies the decoder buffer size, which determines the variability of the output bitrate.
                    "-sn",                            // Disable subtitle.
                    "-dn",                            // Disable data.
                    "-reconnect_delay_max", "60",     // Set the maximum delay in seconds after which to give up reconnecting.
                    "-reconnect_streamed",            // If set then even streamed/non seekable streams will be reconnected on errors.
                    "-reconnect_at_eof",              // If set then eof is treated like an error and causes reconnection, this is useful for live / endless streams.
                    "-max_muxing_queue_size", "1024", // When transcoding audio and/or video streams, ffmpeg will not begin writing into the output until it has one packet for each such stream.
                                                      // While waiting for that to happen, packets for other streams are buffered.
                                                      // This option sets the size of this buffer, in packets, for the matching output stream.
                                                      // The default value of this option should be high enough for most uses, so only touch this option if you are sure that you need it.
                    "-correct_ts_overflow", "1",      // Correct single timestamp overflows if set to 1. Default is 1.
                    "-c:v", "copy",                   // Video codec name.
                    "-c:a", "copy",                   // Audio codec name.
                    "-map", "0",                      // Set input stream mapping.
                }
                .AddIf(isUseProxy, "-http_proxy", httpProxy)
                .AddIf(false, "-f", "segment", "-segment_time", "999999", "-segment_format", "mpegts", "-reset_timestamps", "1")
                .AddIf(true, FileName) // _%03d.ts
                .ToArguments();
                TokenSource = tokenSource ?? new CancellationTokenSource();

                EndTime = DateTime.MinValue;
                StartTime = DateTime.Now;

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

            EndTime = DateTime.Now;
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
