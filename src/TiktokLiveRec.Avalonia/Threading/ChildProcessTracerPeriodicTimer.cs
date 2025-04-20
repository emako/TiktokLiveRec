using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace TiktokLiveRec.Threading;

[SupportedOSPlatform("Windows")]
[SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time")]
[SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression")]
public partial class ChildProcessTracerPeriodicTimer(TimeSpan period) : IDisposable
{
    public static ChildProcessTracerPeriodicTimer Default { get; } = new(TimeSpan.FromMilliseconds(500));

    public PeriodicTimer PeriodicTimer { get; } = new PeriodicTimer(period);
    public CancellationTokenSource? TokenSource { get; protected set; } = null;
    public HashSet<int> TracedChildProcessIds { get; } = [];
    public HashSet<string>? WhiteList { get; set; } = null;

    public void Start(CancellationTokenSource? tokenSource = null)
    {
        TokenSource = tokenSource ?? new CancellationTokenSource();

        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            _ = Task.Factory.StartNew(async () => await StartAsync(TokenSource.Token), TaskCreationOptions.LongRunning);
        }
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

                if (WhiteList != null && WhiteList.Count > 0)
                {
                    if (!WhiteList.Contains(childProcessName))
                    {
                        continue;
                    }
                }

                if (TracedChildProcessIds.Add(childId))
                {
                    using Process childProcess = Process.GetProcessById(childId);

                    if (childProcess != null && childProcess.Handle != nint.Zero)
                    {
                        ChildProcessTracer.Default.AddChildProcess(childProcess.Handle);
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
        PeriodicTimer?.Dispose();
    }

    private static class Interop
    {
        [SuppressMessage("Style", "IDE0305:Simplify collection initialization")]
        public static (int, string)[] GetChildProcessIdAndName(int pid)
        {
            return Process.GetProcesses()
                .Where(p => GetParentProcessId(p.Id) == pid)
                .Select(p => (p.Id, p.ProcessName))
                .ToArray();
        }

        public static unsafe int? GetParentProcessId(int pid)
        {
            nint hProcess = Kernel32.OpenProcess(Kernel32.ACCESS_MASK.GENERIC_READ, false, (uint)pid);

            if (hProcess == nint.Zero)
            {
                return null!;
            }

            NtDll.PROCESS_BASIC_INFORMATION pbi = new();
            int status = NtDll.NtQueryInformationProcess(hProcess, NtDll.PROCESSINFOCLASS.ProcessBasicInformation, (nint)(&pbi), (uint)Marshal.SizeOf<NtDll.PROCESS_BASIC_INFORMATION>(), out _);

            if (status == 0)
            {
                return (int)pbi.InheritedFromUniqueProcessId;
            }
            else
            {
                return null!;
            }
        }

        private static class Kernel32
        {
            [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
            public static extern nint OpenProcess(ACCESS_MASK dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwProcessId);

            [Flags]
            public enum ACCESS_MASK : uint
            {
                DELETE = 0x00010000,
                READ_CONTROL = 0x00020000,
                WRITE_DAC = 0x00040000,
                WRITE_OWNER = 0x00080000,
                SYNCHRONIZE = 0x00100000,
                STANDARD_RIGHTS_REQUIRED = 0x000F0000,
                STANDARD_RIGHTS_READ = 0x00020000,
                STANDARD_RIGHTS_WRITE = 0x00020000,
                STANDARD_RIGHTS_EXECUTE = 0x00020000,
                STANDARD_RIGHTS_ALL = 0x001F0000,
                SPECIFIC_RIGHTS_ALL = 0x0000FFFF,
                ACCESS_SYSTEM_SECURITY = 0x01000000,
                MAXIMUM_ALLOWED = 0x02000000,
                GENERIC_READ = 0x80000000,
                GENERIC_WRITE = 0x40000000,
                GENERIC_EXECUTE = 0x20000000,
                GENERIC_ALL = 0x10000000
            }
        }

        public static class NtDll
        {
            [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
            public static extern int NtQueryInformationProcess(nint hProcess, PROCESSINFOCLASS processInfoClass, nint processInfo, uint processInfoLength, out nint returnLength);

            [StructLayout(LayoutKind.Sequential)]
            public struct PROCESS_BASIC_INFORMATION
            {
                private readonly nint Reserved1;

                public nint PebBaseAddress;

                private readonly nint Reserved2_1;

                private readonly nint Reserved2_2;

                public nint UniqueProcessId;

                private readonly nint Reserved3;

                public readonly int ExitStatus => Reserved1.ToInt32();

                public readonly ref PEB Peb => ref PebBaseAddress.AsRef<PEB>();

                public readonly nuint AffinityMask => (nuint)Reserved2_1;

                public readonly nint BasePriority => Reserved2_2;

                public readonly nuint InheritedFromUniqueProcessId => (nuint)Reserved3;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct PEB
            {
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
                private readonly byte[] Reserved_1;

                public byte BeingDebugged;

                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
                private readonly byte[] Reserved2;

                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
                private readonly nint[] Reserved3;

                public nint Ldr;

                public nint ProcessParameters;

                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
                private readonly nint[] Reserved4;

                private readonly nint AtlThunkSListPtr;

                private readonly nint Reserved5;

                private readonly uint Reserved6;

                private readonly nint Reserved7;

                private readonly uint Reserved8;

                private readonly uint AtlThunkSListPtr32;

                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 45)]
                private readonly nint[] Reserved9;

                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 96)]
                private readonly byte[] Reserved10;

                private readonly nint PostProcessInitRoutine;

                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
                private readonly byte[] Reserved11;

                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
                private readonly nint[] Reserved12;

                public uint SessionId;
            }

            public enum PROCESSINFOCLASS
            {
                ProcessBasicInformation = 0,
                ProcessQuotaLimits,
                ProcessIoCounters,
                ProcessVmCounters,
                ProcessTimes,
                ProcessBasePriority,
                ProcessRaisePriority,
                ProcessDebugPort = 7,
                ProcessExceptionPort,
                ProcessAccessToken,
                ProcessLdtInformation,
                ProcessLdtSize,
                ProcessDefaultHardErrorMode,
                ProcessIoPortHandlers,
                ProcessPooledUsageAndLimits,
                ProcessWorkingSetWatch,
                ProcessUserModeIOPL,
                ProcessEnableAlignmentFaultFixup,
                ProcessPriorityClass,
                ProcessWx86Information,
                ProcessHandleCount,
                ProcessAffinityMask,
                ProcessPriorityBoost,
                ProcessDeviceMap,
                ProcessSessionInformation,
                ProcessForegroundInformation,
                ProcessWow64Information = 26,
                ProcessImageFileName = 27,
                ProcessLUIDDeviceMapsEnabled,
                ProcessBreakOnTermination = 29,
                ProcessDebugObjectHandle,
                ProcessDebugFlags,
                ProcessHandleTracing,
                ProcessIoPriority,
                ProcessExecuteFlags,
                ProcessResourceManagement,
                ProcessCookie,
                ProcessImageInformation,
                ProcessCycleTime,
                ProcessPagePriority,
                ProcessInstrumentationCallback,
                ProcessThreadStackAllocation,
                ProcessWorkingSetWatchEx,
                ProcessImageFileNameWin32,
                ProcessImageFileMapping,
                ProcessAffinityUpdateMode,
                ProcessMemoryAllocationMode,
                ProcessGroupInformation,
                ProcessTokenVirtualizationEnabled,
                ProcessConsoleHostProcess,
                ProcessWindowInformation,
                ProcessHandleInformation,
                ProcessMitigationPolicy,
                ProcessDynamicFunctionTableInformation,
                ProcessHandleCheckingMode,
                ProcessKeepAliveCount,
                ProcessRevokeFileHandles,
                ProcessWorkingSetControl,
                ProcessHandleTable,
                ProcessCheckStackExtentsMode,
                ProcessCommandLineInformation,
                ProcessProtectionInformation,
                ProcessMemoryExhaustion,
                ProcessFaultInformation,
                ProcessTelemetryIdInformation,
                ProcessCommitReleaseInformation,
                ProcessDefaultCpuSetsInformation,
                ProcessAllowedCpuSetsInformation,
                ProcessSubsystemProcess,
                ProcessJobMemoryInformation,
                ProcessInPrivate,
                ProcessRaiseUMExceptionOnInvalidHandleClose,
                ProcessIumChallengeResponse,
                ProcessChildProcessInformation,
                ProcessHighGraphicsPriorityInformation,
                ProcessSubsystemInformation = 75,
                ProcessEnergyValues,
                ProcessActivityThrottleState,
                ProcessActivityThrottlePolicy,
                ProcessWin32kSyscallFilterInformation,
                ProcessDisableSystemAllowedCpuSets,
                ProcessWakeInformation,
                ProcessEnergyTrackingState,
                ProcessManageWritesToExecutableMemory,
                ProcessCaptureTrustletLiveDump,
                ProcessTelemetryCoverage,
                ProcessEnclaveInformation,
                ProcessEnableReadWriteVmLogging,
                ProcessUptimeInformation,
                ProcessImageSection,
                ProcessDebugAuthInformation,
                ProcessSystemResourceManagement,
                ProcessSequenceNumber,
                ProcessLoaderDetour,
                ProcessSecurityDomainInformation,
                ProcessCombineSecurityDomainsInformation,
                ProcessEnableLogging,
                ProcessLeapSecondInformation,
                ProcessFiberShadowStackAllocation,
                ProcessFreeFiberShadowStackAllocation,
                ProcessAltSystemCallInformation,
                ProcessDynamicEHContinuationTargets,
            }

            public enum SUBSYSTEM_INFORMATION_TYPE
            {
                SubsystemInformationTypeWin32,

                SubsystemInformationTypeWSL,

                MaxSubsystemInformationType,
            }
        }
    }
}

/// <summary>
/// Make sures that child processes are automatically terminated
/// if the parent process exits unexpectedly.
/// </summary>
[SupportedOSPlatform("Windows")]
[SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time")]
[SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression")]
internal sealed class ChildProcessTracer
{
    public static ChildProcessTracer Default { get; } = new();

    private static class Kernel32
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AssignProcessToJobObject([In] nint hJob, [In] nint hProcess);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern nint CreateJobObject([In, Optional] SECURITY_ATTRIBUTES? lpJobAttributes, [In, Optional] string? lpName);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetInformationJobObject(nint hJob, JOBOBJECTINFOCLASS JobObjectInfoClass, nint lpJobObjectInfo, uint cbJobObjectInfoLength);

        [StructLayout(LayoutKind.Sequential)]
        public class SECURITY_ATTRIBUTES
        {
            public int nLength = Marshal.SizeOf<SECURITY_ATTRIBUTES>();

            public nint lpSecurityDescriptor;

            [MarshalAs(UnmanagedType.Bool)]
            public bool bInheritHandle;
        }

        public enum JOBOBJECTINFOCLASS
        {
            JobObjectBasicAccountingInformation = 1,
            JobObjectBasicLimitInformation,
            JobObjectBasicProcessIdList,
            JobObjectBasicUIRestrictions,
            JobObjectSecurityLimitInformation,
            JobObjectEndOfJobTimeInformation,
            JobObjectAssociateCompletionPortInformation,
            JobObjectBasicAndIoAccountingInformation,
            JobObjectExtendedLimitInformation,
            JobObjectJobSetInformation,
            JobObjectGroupInformation,
            JobObjectNotificationLimitInformation,
            JobObjectLimitViolationInformation,
            JobObjectGroupInformationEx,
            JobObjectCpuRateControlInformation,
            JobObjectCompletionFilter,
            JobObjectCompletionCounter,
            JobObjectReserved1Information = 18,
            JobObjectReserved2Information,
            JobObjectReserved3Information,
            JobObjectReserved4Information,
            JobObjectReserved5Information,
            JobObjectReserved6Information,
            JobObjectReserved7Information,
            JobObjectReserved8Information,
            JobObjectReserved9Information,
            JobObjectReserved10Information,
            JobObjectReserved11Information,
            JobObjectReserved12Information,
            JobObjectReserved13Information,
            JobObjectReserved14Information = 31,
            JobObjectNetRateControlInformation,
            JobObjectNotificationLimitInformation2,
            JobObjectLimitViolationInformation2,
            JobObjectCreateSilo,
            JobObjectSiloBasicInformation,
            JobObjectReserved15Information = 37,
            JobObjectReserved16Information = 38,
            JobObjectReserved17Information = 39,
            JobObjectReserved18Information = 40,
            JobObjectReserved19Information = 41,
            JobObjectReserved20Information = 42,
            JobObjectReserved21Information = 43,
            JobObjectReserved22Information = 44,
            JobObjectReserved23Information = 45,
            JobObjectReserved24Information = 46,
            JobObjectReserved25Information = 47,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct JOBOBJECT_EXTENDED_LIMIT_INFORMATION
        {
            public JOBOBJECT_BASIC_LIMIT_INFORMATION BasicLimitInformation;
            public IO_COUNTERS IoInfo;
            public SizeT ProcessMemoryLimit;
            public SizeT JobMemoryLimit;
            public SizeT PeakProcessMemoryUsed;
            public SizeT PeakJobMemoryUsed;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct JOBOBJECT_BASIC_LIMIT_INFORMATION
        {
            public TimeSpan PerProcessUserTimeLimit;
            public TimeSpan PerJobUserTimeLimit;
            public JOBOBJECT_LIMIT_FLAGS LimitFlags;
            public SizeT MinimumWorkingSetSize;
            public SizeT MaximumWorkingSetSize;
            public uint ActiveProcessLimit;
            public nuint Affinity;
            public uint PriorityClass;
            public uint SchedulingClass;
        }

        [Flags]
        public enum JOBOBJECT_LIMIT_FLAGS
        {
            JOB_OBJECT_LIMIT_WORKINGSET = 0x00000001,
            JOB_OBJECT_LIMIT_PROCESS_TIME = 0x00000002,
            JOB_OBJECT_LIMIT_JOB_TIME = 0x00000004,
            JOB_OBJECT_LIMIT_ACTIVE_PROCESS = 0x00000008,
            JOB_OBJECT_LIMIT_AFFINITY = 0x00000010,
            JOB_OBJECT_LIMIT_PRIORITY_CLASS = 0x00000020,
            JOB_OBJECT_LIMIT_PRESERVE_JOB_TIME = 0x00000040,
            JOB_OBJECT_LIMIT_SCHEDULING_CLASS = 0x00000080,
            JOB_OBJECT_LIMIT_PROCESS_MEMORY = 0x00000100,
            JOB_OBJECT_LIMIT_JOB_MEMORY = 0x00000200,
            JOB_OBJECT_LIMIT_DIE_ON_UNHANDLED_EXCEPTION = 0x00000400,
            JOB_OBJECT_LIMIT_BREAKAWAY_OK = 0x00000800,
            JOB_OBJECT_LIMIT_SILENT_BREAKAWAY_OK = 0x00001000,
            JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE = 0x00002000,
            JOB_OBJECT_LIMIT_SUBSET_AFFINITY = 0x00004000,
            JOB_OBJECT_LIMIT_JOB_MEMORY_LOW = 0x00008000,
            JOB_OBJECT_LIMIT_JOB_READ_BYTES = 0x00010000,
            JOB_OBJECT_LIMIT_JOB_WRITE_BYTES = 0x00020000,
            JOB_OBJECT_LIMIT_RATE_CONTROL = 0x00040000,
            JOB_OBJECT_LIMIT_CPU_RATE_CONTROL = JOB_OBJECT_LIMIT_RATE_CONTROL,
            JOB_OBJECT_LIMIT_IO_RATE_CONTROL = 0x00080000,
            JOB_OBJECT_LIMIT_NET_RATE_CONTROL = 0x00100000,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IO_COUNTERS
        {
            public ulong ReadOperationCount;
            public ulong WriteOperationCount;
            public ulong OtherOperationCount;
            public ulong ReadTransferCount;
            public ulong WriteTransferCount;
            public ulong OtherTransferCount;
        }

        public struct SizeT
        {
            private nuint val;

            public ulong Value
            {
                readonly get => val;
                private set => val = new UIntPtr(value);
            }
        }
    }

    private readonly nint hJob;

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

    /// <summary>
    /// Adds a process to the tracking job. If the parent process is terminated, this process will also be automatically terminated.
    /// </summary>
    /// <param name="hProcess">The child process to be tracked.</param>
    /// <exception cref="ArgumentNullException">Thrown when the process argument is null.</exception>
    public void AddChildProcess(nint hProcess)
    {
        if (!Kernel32.AssignProcessToJobObject(hJob, hProcess))
        {
            Debug.WriteLine($"Failed to assign process to job object. Error: {Marshal.GetLastWin32Error()}");
        }
    }
}

public static class UnsafeExtensions
{
    public static unsafe ref T AsRef<T>(this nint address) where T : struct
    {
#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
        return ref *(T*)address;
#pragma warning restore CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
    }
}
