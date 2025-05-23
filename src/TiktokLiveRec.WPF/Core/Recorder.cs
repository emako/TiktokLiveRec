using CommunityToolkit.Mvvm.Messaging;
using Flucli;
using Flucli.Utils.Extensions;
using System.Diagnostics;
using System.Text;
using TiktokLiveRec.Extensions;
using TiktokLiveRec.Models;

namespace TiktokLiveRec.Core;

public sealed class Recorder
{
    public RecordStatus RecordStatus { get; internal set; } = RecordStatus.Initialized;

    public CancellationTokenSource? TokenSource { get; private set; } = null;

    public string? Url { get; set; } = null;

    public string? FileName { get; set; } = null;

    public string? Parameters { get; set; } = null;

    public DateTime StartTime { get; private set; } = DateTime.MinValue;

    public DateTime EndTime { get; private set; } = DateTime.MinValue;

    public bool IsToSegment { get; set; } = false;

    public Task Start(RecorderStartInfo startInfo, CancellationTokenSource? tokenSource = null)
    {
        if (RecordStatus == RecordStatus.Recording)
        {
            // Already recording
            return Task.CompletedTask;
        }

        RecordStatus = RecordStatus.Recording;

        // Start a recording task that does not use the default ThreadPool.
        return Task.Factory.StartNew(async () =>
        {
            try
            {
                string? recorderPath = SearchFileHelper.SearchFiles(".", "ffmpeg[\\.exe]").FirstOrDefault();

                if (recorderPath == null)
                {
                    // Recorder not found so you should reinstall the program.
                    RecordStatus = RecordStatus.NotRecording;
                    return;
                }

                string saveFolder = SaveFolderHelper.GetSaveFolder(Configurations.SaveFolder.Get());

                saveFolder = Path.Combine(saveFolder,
                    Configurations.SaveFolderDistinguishedByAuthors.Get()
                        ? startInfo.NickName.SanitizeFileName().ReplaceTrailingDotsWithUnderscores()
                        : string.Empty
                );
                if (!Directory.Exists(saveFolder))
                {
                    Directory.CreateDirectory(saveFolder);
                }

                string userAgent = Configurations.UserAgent.Get();
                string httpProxy = Configurations.ProxyUrl.Get();
                bool isUseProxy = Configurations.IsUseProxy.Get() && !string.IsNullOrWhiteSpace(httpProxy);
                int segmentTime = Configurations.SegmentTime.Get();
                bool isToSegment = Configurations.IsToSegment.Get() && segmentTime > 0;

                IsToSegment = isToSegment;

                if (!string.IsNullOrWhiteSpace(startInfo.HlsUrl))
                {
                    Url = startInfo.HlsUrl;

                    if (isToSegment)
                    {
                        FileName = Path.Combine(saveFolder, $"{startInfo.NickName.SanitizeFileName()}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}_%03d.ts");
                    }
                    else
                    {
                        FileName = Path.Combine(saveFolder, $"{startInfo.NickName.SanitizeFileName()}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.ts");
                    }
                }
                else
                {
                    Url = startInfo.FlvUrl;

                    if (isToSegment)
                    {
                        FileName = Path.Combine(saveFolder, $"{startInfo.NickName.SanitizeFileName()}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}_%03d.ts");
                    }
                    else
                    {
                        FileName = Path.Combine(saveFolder, $"{startInfo.NickName.SanitizeFileName()}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}_%03d.flv");
                    }
                }

                if (string.IsNullOrWhiteSpace(userAgent))
                {
                    userAgent = "Mozilla/5.0 (Linux; Android 11; SAMSUNG SM-G973U) AppleWebKit/537.36 ("
                              + "KHTML, like Gecko) SamsungBrowser/14.2 Chrome/87.0.4280.141 Mobile "
                              + "Safari/537.36";
                }

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
                    "-i", Url,                        // Input infile.
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
                .AddIf(isToSegment,
                    "-f", "segment",
                    "-segment_time", segmentTime.ToString(), // in secs
                    "-segment_format", "mpegts",
                    "-reset_timestamps", "1"
                )
                .AddIf(true, FileName) // _%03d
                .ToArguments();
                TokenSource = tokenSource ?? new CancellationTokenSource();

                EndTime = DateTime.MinValue;
                StartTime = DateTime.Now;

                CliResult result = await recorderPath
                    .WithArguments(Parameters)
                    .WithEnvironmentVariable(
                    [
                        (isUseProxy ? "http_proxy" : "__TIKTOKLIVEREC_IGNORE_HTTP_PROXY__", "http://" + httpProxy),
                        (isUseProxy ? "https_proxy" : "__TIKTOKLIVEREC_IGNORE_HTTPS_PROXY__", "http://" + httpProxy),
                    ])
                    .WithStandardErrorPipe(PipeTarget.ToDelegate(OnStandardErrorReceived, Encoding.UTF8))
                    .WithStandardOutputPipe(PipeTarget.ToDelegate(OnStandardOutputReceived, Encoding.UTF8))
                    .ExecuteAsync(cancellationToken: TokenSource.Token);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            try
            {
                // Converter to target format if recorded.
                if (File.Exists(FileName))
                {
                    string formatArrow = Configurations.RecordFormat.Get();

                    if (!string.IsNullOrEmpty(formatArrow) && formatArrow.Contains("->"))
                    {
                        formatArrow = "." + formatArrow.Split('>')[1].Trim().ToLower();

                        // Execute the converter asynchronously.
                        // So don't use await here.
                        _ = new Converter().ExecuteAsync(FileName, formatArrow)
                            .ContinueWith(task =>
                            {
                                // Remove only the conversion is successful.
                                if (task.Result && Configurations.IsRemoveTs.Get())
                                {
                                    try
                                    {
                                        File.Delete(FileName);
                                        FileName = Path.ChangeExtension(FileName, formatArrow);
                                    }
                                    catch (Exception e)
                                    {
                                        Debug.WriteLine(e);
                                    }
                                }
                            })
                            .ConfigureAwait(false);
                    }
                }
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
        // TODO
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
        // TODO
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

    [Obsolete("Should retry recording instead of pushing an Error Status")]
    Error,
}

public record RecorderStartInfo
{
    public string NickName { get; set; } = string.Empty;

    public string FlvUrl { get; set; } = string.Empty;

    public string HlsUrl { get; set; } = string.Empty;
}

file static class FileNameSanitizer
{
    /// <summary>
    /// Sanitizes the input file name by replacing invalid characters with underscores.
    /// </summary>
    /// <param name="fileName">The input file name.</param>
    /// <returns>The sanitized file name.</returns>
    public static string SanitizeFileName(this string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name cannot be null or whitespace.", nameof(fileName));

        // Get invalid file name characters.
        char[] invalidChars = Path.GetInvalidFileNameChars();

        // Replace each invalid character with an underscore.
        return string.Concat(fileName.Select(ch => invalidChars.Contains(ch) ? '_' : ch));
    }

    /// <summary>
    /// Replaces all trailing periods (.) in a string with underscores (_).
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <returns>The modified string with trailing periods replaced by underscores.</returns>
    public static string ReplaceTrailingDotsWithUnderscores(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        int i = input.Length - 1;
        while (i >= 0 && input[i] == '.')
        {
            i--;
        }

        // Replace trailing dots with underscores
        return string.Concat(input.AsSpan(0, i + 1), new string('_', input.Length - i - 1));
    }
}

file static class NoLinqExtension
{
    public static List<string> AddIf(this List<string> self, bool condition, params string[] items)
    {
        if (condition)
        {
            foreach (string item in items)
            {
                self.Add(item);
            }
        }

        return self;
    }
}
