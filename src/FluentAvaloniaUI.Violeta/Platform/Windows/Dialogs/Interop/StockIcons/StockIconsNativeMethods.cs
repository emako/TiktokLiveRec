using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.StockIcons;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.StockIcons;

[SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time")]
internal static class StockIconsNativeMethods
{
    [Flags]
    internal enum StockIconOptions
    {
        Large = 0x000000000,
        Small = 0x000000001,
        ShellSize = 0x000000004,
        Handle = 0x000000100,
        SystemIndex = 0x000004000,
        LinkOverlay = 0x000008000,
        Selected = 0x000010000,
    }

    [PreserveSig]
    [DllImport("shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = false)]
    internal static extern HResult SHGetStockIconInfo(
        StockIconIdentifier identifier,
        StockIconOptions flags,
        ref StockIconInfo info);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct StockIconInfo
    {
        internal uint StuctureSize;
        internal nint Handle;
        internal int ImageIndex;
        internal int Identifier;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        internal string Path;
    }
}
