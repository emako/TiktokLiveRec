using CommunityToolkit.Mvvm.Messaging;
using Flucli;
using Flucli.Utils.Extensions;
using System.Diagnostics;
using System.Text;
using TiktokLiveRec.Extensions;
using TiktokLiveRec.Models;

namespace TiktokLiveRec.Core;

public sealed class Converter
{
    public async Task<bool> ExecuteAsync(string sourceFileName, string targetFormat, CancellationTokenSource? tokenSource = null)
    {
        _ = sourceFileName ?? throw new ArgumentNullException(nameof(sourceFileName));
        _ = targetFormat ?? throw new ArgumentNullException(nameof(targetFormat));

        string? recorderPath = SearchFileHelper.SearchFiles(".", "ffmpeg[\\.exe]").FirstOrDefault();

        if (recorderPath == null)
        {
            // Error on Converter not found
            return false;
        }

        FileInfo sourceFileInfo = new(sourceFileName);

        if (!sourceFileInfo.Exists)
        {
            return false;
        }

        if (sourceFileInfo.Extension.Equals(targetFormat, StringComparison.CurrentCultureIgnoreCase))
        {
            return false;
        }

        string targetFileName = Path.ChangeExtension(sourceFileName, targetFormat);
        string parameters = string.Empty;

        // From TS to other format
        if (sourceFileInfo.Extension.Equals(".ts", StringComparison.CurrentCultureIgnoreCase))
        {
            parameters = new List<string>()
            {
                "-y",
                "-fflags", "+genpts",
                "-i", sourceFileName,
                "-c", "copy", targetFileName,
            }.ToArguments();
        }
        else if (sourceFileInfo.Extension.Equals(".flv", StringComparison.CurrentCultureIgnoreCase))
        {
            parameters = new List<string>()
            {
                "-y",
                "-i", sourceFileName,
                "-c", "copy", targetFileName,
            }.ToArguments();
        }

        CliResult result = await recorderPath
            .WithArguments(parameters)
            .WithStandardErrorPipe(PipeTarget.ToDelegate(OnStandardErrorReceived, Encoding.UTF8))
            .WithStandardOutputPipe(PipeTarget.ToDelegate(OnStandardOutputReceived, Encoding.UTF8))
            .ExecuteAsync(cancellationToken: tokenSource?.Token ?? default);

        Debug.WriteLine($"[Converter] exit code is {result.ExitCode}.");

        return result.IsSuccess;
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
