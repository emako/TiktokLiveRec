using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Vanara.PInvoke;

namespace TiktokLiveRec.Threading;

public partial class ChildProcessTracerPeriodicTimer(TimeSpan period) : IDisposable
{
    public static ChildProcessTracerPeriodicTimer Default { get; } = new(TimeSpan.FromMilliseconds(500));

    public ChildProcessTracer Tracer { get; } = new ChildProcessTracer();
    public PeriodicTimer PeriodicTimer { get; } = new PeriodicTimer(period);
    public CancellationTokenSource? TokenSource { get; protected set; } = null;
    public HashSet<int> TracedChildProcessIds { get; } = [];

    public void Start(CancellationTokenSource? tokenSource = null)
    {
        TokenSource = tokenSource ?? new CancellationTokenSource();

        _ = Task.Factory.StartNew(async () => await StartAsync(TokenSource.Token), TaskCreationOptions.LongRunning);
    }

    private async Task StartAsync(CancellationToken cancellationToken = default)
    {
        TracedChildProcessIds.Clear();

        while (await PeriodicTimer.WaitForNextTickAsync(cancellationToken))
        {
            (int Id, string ProcessName)[] children = Interop.GetChildProcessIdAndName(Environment.ProcessId);

            foreach ((int childId, string childProcessName) in children)
            {
                // Skip owner console host process
                if (childProcessName == "conhost")
                {
                    continue;
                }

                if (TracedChildProcessIds.Add(childId))
                {
                    using Process childProcess = Process.GetProcessById(childId);

                    if (childProcess != null && childProcess.Handle != IntPtr.Zero)
                    {
                        Tracer.AddChildProcess(childProcess.Handle);
                    }
                }
            }

            // Check if child process is still alive
            TracedChildProcessIds.RemoveWhere(tracedChildProcessId =>
               !children.Any(child => child.Id == tracedChildProcessId)
            );
        }
    }

    public void Stop()
    {
        TokenSource?.Cancel();
    }

    [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize")]
    [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression")]
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
public partial class ChildProcessTracer : IDisposable
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
