using System.Runtime.InteropServices;

namespace TiktokLiveRec.Core;

public static class SystemVolume
{
    public static float GetMasterVolume()
    {
        IAudioEndpointVolume masterVol = null!;

        try
        {
            masterVol = SystemVolumeService.GetMasterVolumeObject();
            if (masterVol == null)
            {
                return -1f;
            }

            masterVol.GetMasterVolumeLevelScalar(out var volumeLevel);
            return volumeLevel * 100f;
        }
        finally
        {
            if (masterVol != null)
            {
                _ = Marshal.ReleaseComObject(masterVol);
            }
        }
    }

    public static void SetMasterVolume(float newLevel)
    {
        IAudioEndpointVolume masterVol = null!;
        try
        {
            masterVol = SystemVolumeService.GetMasterVolumeObject();
            if (masterVol == null)
            {
                return;
            }

            _ = masterVol.SetMasterVolumeLevelScalar(newLevel / 100f, Guid.Empty);
        }
        finally
        {
            if (masterVol != null)
            {
                _ = Marshal.ReleaseComObject(masterVol);
            }
        }
    }

    public static void SetMasterVolumeMute(bool isMuted)
    {
        IAudioEndpointVolume masterVol = null!;

        try
        {
            masterVol = SystemVolumeService.GetMasterVolumeObject();
            if (masterVol == null)
            {
                return;
            }

            _ = masterVol.SetMute(isMuted, Guid.Empty);
        }
        finally
        {
            if (masterVol != null)
            {
                _ = Marshal.ReleaseComObject(masterVol);
            }
        }
    }
}

file static class SystemVolumeService
{
    public static IAudioEndpointVolume GetMasterVolumeObject()
    {
        IMMDeviceEnumerator deviceEnumerator = null!;
        IMMDevice speakers = null!;

        try
        {
            deviceEnumerator = (IMMDeviceEnumerator)new MMDeviceEnumerator();
            _ = deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, out speakers);

            Guid iidIAudioEndpointVolume = typeof(IAudioEndpointVolume).GUID;
            _ = speakers.Activate(ref iidIAudioEndpointVolume, 0, IntPtr.Zero, out object o);
            IAudioEndpointVolume masterVol = (IAudioEndpointVolume)o;

            return masterVol;
        }
        finally
        {
            if (speakers != null)
            {
                _ = Marshal.ReleaseComObject(speakers);
            }

            if (deviceEnumerator != null)
            {
                _ = Marshal.ReleaseComObject(deviceEnumerator);
            }
        }
    }
}

[ComImport]
[Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
file class MMDeviceEnumerator
{
}

file enum EDataFlow
{
    eRender,
    eCapture,
    eAll,
    EDataFlow_enum_count
}

file enum ERole
{
    eConsole,
    eMultimedia,
    eCommunications,
    ERole_enum_count
}

[Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
file interface IMMDeviceEnumerator
{
    public int NotImpl1();

    [PreserveSig]
    public int GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role, out IMMDevice ppDevice);
}

[Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
file interface IMMDevice
{
    [PreserveSig]
    public int Activate(ref Guid iid, int dwClsCtx, IntPtr pActivationParams, [MarshalAs(UnmanagedType.IUnknown)] out object ppInterface);
}

[Guid("E2F5BB11-0570-40CA-ACDD-3AA01277DEE8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
file interface IAudioSessionEnumerator
{
    [PreserveSig]
    public int GetCount(out int SessionCount);

    [PreserveSig]
    public int GetSession(int SessionCount, out IAudioSessionControl2 Session);
}

[Guid("bfb7ff88-7239-4fc9-8fa2-07c950be9c6d"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
file interface IAudioSessionControl2
{
    [PreserveSig]
    public int NotImpl0();

    [PreserveSig]
    public int GetDisplayName([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);

    [PreserveSig]
    public int SetDisplayName([MarshalAs(UnmanagedType.LPWStr)] string Value, [MarshalAs(UnmanagedType.LPStruct)] Guid EventContext);

    [PreserveSig]
    public int GetIconPath([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);

    [PreserveSig]
    public int SetIconPath([MarshalAs(UnmanagedType.LPWStr)] string Value, [MarshalAs(UnmanagedType.LPStruct)] Guid EventContext);

    [PreserveSig]
    public int GetGroupingParam(out Guid pRetVal);

    [PreserveSig]
    public int SetGroupingParam([MarshalAs(UnmanagedType.LPStruct)] Guid Override, [MarshalAs(UnmanagedType.LPStruct)] Guid EventContext);

    [PreserveSig]
    public int NotImpl1();

    [PreserveSig]
    public int NotImpl2();

    [PreserveSig]
    public int GetSessionIdentifier([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);

    [PreserveSig]
    public int GetSessionInstanceIdentifier([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);

    [PreserveSig]
    public int GetProcessId(out int pRetVal);

    [PreserveSig]
    public int IsSystemSoundsSession();

    [PreserveSig]
    public int SetDuckingPreference(bool optOut);
}

[Guid("5CDF2C82-841E-4546-9722-0CF74078229A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
file interface IAudioEndpointVolume
{
    [PreserveSig]
    public int NotImpl1();

    [PreserveSig]
    public int NotImpl2();

    [PreserveSig]
    public int GetChannelCount([Out][MarshalAs(UnmanagedType.U4)] out uint channelCount);

    [PreserveSig]
    public int SetMasterVolumeLevel([In][MarshalAs(UnmanagedType.R4)] float level, [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

    [PreserveSig]
    public int SetMasterVolumeLevelScalar([In][MarshalAs(UnmanagedType.R4)] float level, [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

    [PreserveSig]
    public int GetMasterVolumeLevel([Out][MarshalAs(UnmanagedType.R4)] out float level);

    [PreserveSig]
    public int GetMasterVolumeLevelScalar([Out][MarshalAs(UnmanagedType.R4)] out float level);

    [PreserveSig]
    public int SetChannelVolumeLevel([In][MarshalAs(UnmanagedType.U4)] uint channelNumber, [In][MarshalAs(UnmanagedType.R4)] float level, [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

    [PreserveSig]
    public int SetChannelVolumeLevelScalar([In][MarshalAs(UnmanagedType.U4)] uint channelNumber, [In][MarshalAs(UnmanagedType.R4)] float level, [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

    [PreserveSig]
    public int GetChannelVolumeLevel([In][MarshalAs(UnmanagedType.U4)] uint channelNumber, [Out][MarshalAs(UnmanagedType.R4)] out float level);

    [PreserveSig]
    public int GetChannelVolumeLevelScalar([In][MarshalAs(UnmanagedType.U4)] uint channelNumber, [Out][MarshalAs(UnmanagedType.R4)] out float level);

    [PreserveSig]
    public int SetMute([In][MarshalAs(UnmanagedType.Bool)] bool isMuted, [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

    [PreserveSig]
    public int GetMute([Out][MarshalAs(UnmanagedType.Bool)] out bool isMuted);

    [PreserveSig]
    public int GetVolumeStepInfo([Out][MarshalAs(UnmanagedType.U4)] out uint step, [Out][MarshalAs(UnmanagedType.U4)] out uint stepCount);

    [PreserveSig]
    public int VolumeStepUp([In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

    [PreserveSig]
    public int VolumeStepDown([In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

    [PreserveSig]
    public int QueryHardwareSupport([Out][MarshalAs(UnmanagedType.U4)] out uint hardwareSupportMask);

    [PreserveSig]
    public int GetVolumeRange([Out][MarshalAs(UnmanagedType.R4)] out float volumeMin, [Out][MarshalAs(UnmanagedType.R4)] out float volumeMax, [Out][MarshalAs(UnmanagedType.R4)] out float volumeStep);
}
