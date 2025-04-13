using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Natives;

[SupportedOSPlatform("Windows")]
[SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time")]
internal static class Shell32
{
    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    public static extern bool Shell_NotifyIcon(int dwMessage, ref NotifyIconData pnid);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct NotifyIconData
    {
        public int cbSize;
        public nint hWnd;
        public int uID;
        public int uFlags;
        public int uCallbackMessage;
        public nint hIcon;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szTip;

        public uint dwState;
        public uint dwStateMask;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szInfo;

        public uint uTimeoutOrVersion;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string szInfoTitle;

        public uint dwInfoFlags;

        public Guid guidItem;

        public nint hBalloonIcon;
    }

    [Flags]
    public enum NotifyIconState : uint
    {
        NIS_HIDDEN = 0x1,
        NIS_SHAREDICON = 0x2
    }

    [Flags]
    public enum NotifyIconFlags : uint
    {
        NIF_MESSAGE = 0x00000001,
        NIF_ICON = 0x00000002,
        NIF_TIP = 0x00000004,
        NIF_STATE = 0x00000008,
        NIF_INFO = 0x00000010,
        NIF_GUID = 0x00000020,
        NIF_REALTIME = 0x00000040,
        NIF_SHOWTIP = 0x00000080
    }

    public enum NOTIFY_COMMAND : uint
    {
        NIM_ADD = 0x00000000,
        NIM_MODIFY = 0x00000001,
        NIM_DELETE = 0x00000002,
        NIM_SETVERSION = 0x00000004
    }
}
