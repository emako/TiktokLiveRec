using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Vanara.PInvoke;

namespace TiktokLiveRec.Threading;

public partial class ChildProcessTrackPeriodicTimer : IDisposable
{
    public ChildProcessTracer Tracer { get; }
    public PeriodicTimer PeriodicTimer { get; }

    public ChildProcessTrackPeriodicTimer(TimeSpan period)
    {
        Tracer = new ChildProcessTracer();
        PeriodicTimer = new PeriodicTimer(period);
    }

    [Obsolete]
    public async ValueTask AttachChildConsoleAsync(CancellationToken cancellationToken = default)
    {
        bool ret = Kernel32.AllocConsole();

        while (await PeriodicTimer.WaitForNextTickAsync())
        {
            (int, string)[] children = Interop.GetChildProcessIdAndName(Environment.ProcessId);

            foreach ((int Id, string ProcessName) child in children)
            {
                if (child.ProcessName == "conhost")
                {
                    continue;
                }

                if (Kernel32.AttachConsole((uint)child.Id))
                {
                    Debug.WriteLine("Console attached.");
                }
                else
                {
                    Debug.WriteLine("Failed to attach console.");
                }
            }
        }

        ret = Kernel32.FreeConsole();
    }

    public async ValueTask AttachChildProcessAsync(CancellationToken cancellationToken = default)
    {
        while (await PeriodicTimer.WaitForNextTickAsync())
        {
            (int, string)[] children = Interop.GetChildProcessIdAndName(Environment.ProcessId);

            foreach ((int Id, string ProcessName) child in children)
            {
                // Skip owner console host process
                if (child.ProcessName == "conhost")
                {
                    continue;
                }

                Tracer.AddChildProcess(Process.GetProcessById(child.Id).Handle);
            }
        }
    }

    [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize")]
    public void Dispose()
    {
        Tracer.Dispose();
        PeriodicTimer?.Dispose();
    }
}

/// <summary>
/// Make sures that child processes are automatically terminated
/// if the parent process exits unexpectedly.
/// </summary>
public class ChildProcessTracer : IDisposable
{
    private readonly Kernel32.SafeHJOB? hJob;

    public ChildProcessTracer()
    {
        if (Environment.OSVersion.Version < new Version(6, 2))
        {
            return;
        }

        hJob = Kernel32.CreateJobObject(null, $"{nameof(ChildProcessTracer)}-{Environment.ProcessId}");

        Kernel32.JOBOBJECT_EXTENDED_LIMIT_INFORMATION extendedInfo = new()
        {
            BasicLimitInformation = new Kernel32.JOBOBJECT_BASIC_LIMIT_INFORMATION
            {
                LimitFlags = Kernel32.JOBOBJECT_LIMIT_FLAGS.JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE
            }
        };

        int length = Marshal.SizeOf(extendedInfo);
        nint extendedInfoPtr = Marshal.AllocHGlobal(length);

        try
        {
            Marshal.StructureToPtr(extendedInfo, extendedInfoPtr, false);

            if (!Kernel32.SetInformationJobObject(hJob, Kernel32.JOBOBJECTINFOCLASS.JobObjectExtendedLimitInformation, extendedInfoPtr, (uint)length))
            {
                Debug.WriteLine($"Failed to set information for job object. Error: {Marshal.GetLastWin32Error()}");
            }
        }
        finally
        {
            // Free allocated memory after job object is set.
            Marshal.FreeHGlobal(extendedInfoPtr);
        }
    }

    public void Dispose()
    {
        hJob?.Dispose();
    }

    /// <summary>
    /// Adds a process to the tracking job. If the parent process is terminated, this process will also be automatically terminated.
    /// </summary>
    /// <param name="hProcess">The child process to be tracked.</param>
    /// <exception cref="ArgumentNullException">Thrown when the process argument is null.</exception>
    public void AddChildProcess(nint hProcess)
    {
        if (hJob != null && !hJob.IsInvalid)
        {
            if (!Kernel32.AssignProcessToJobObject(hJob, hProcess))
            {
                Debug.WriteLine($"Failed to assign process to job object. Error: {Marshal.GetLastWin32Error()}");
            }
        }
    }
}
