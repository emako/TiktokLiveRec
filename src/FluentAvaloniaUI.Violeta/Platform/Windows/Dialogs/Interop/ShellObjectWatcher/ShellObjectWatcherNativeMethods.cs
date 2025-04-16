using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.ShellExtensions;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Versioning;

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.ShellObjectWatcher;

[SupportedOSPlatform("Windows")]
[StructLayout(LayoutKind.Sequential)]
[SuppressMessage("Style", "IDE0251:Make member 'readonly'")]
public struct Message
{
    private readonly nint windowHandle;
    private readonly uint msg;
    private readonly nint wparam;
    private readonly nint lparam;
    private readonly int time;
    private POINT point;

    internal Message(nint windowHandle, uint msg, nint wparam, nint lparam, int time, POINT point)
        : this()
    {
        this.windowHandle = windowHandle;
        this.msg = msg;
        this.wparam = wparam;
        this.lparam = lparam;
        this.time = time;
        this.point = point;
    }

    public nint LParam => lparam;

    public uint Msg => msg;

    public POINT Point => point;

    public int Time => time;

    public nint WindowHandle => windowHandle;

    public nint WParam => wparam;

    public static bool operator !=(Message first, Message second) => !(first == second);

    public static bool operator ==(Message first, Message second) => first.WindowHandle == second.WindowHandle
            && first.Msg == second.Msg
            && first.WParam == second.WParam
            && first.LParam == second.LParam
            && first.Time == second.Time
            && first.Point == second.Point;

    public override bool Equals(object obj) => (obj != null && obj is Message) ? this == (Message)obj : false;

    [SuppressMessage("Style", "IDE0070:Use 'System.HashCode'")]
    public override int GetHashCode()
    {
        var hash = WindowHandle.GetHashCode();
        hash = hash * 31 + Msg.GetHashCode();
        hash = hash * 31 + WParam.GetHashCode();
        hash = hash * 31 + LParam.GetHashCode();
        hash = hash * 31 + Time.GetHashCode();
        hash = hash * 31 + Point.GetHashCode();
        return hash;
    }
}

[SupportedOSPlatform("Windows")]
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct WindowClassEx
{
    internal uint Size;
    internal uint Style;

    internal ShellObjectWatcherNativeMethods.WndProcDelegate WndProc;

    internal int ExtraClassBytes;
    internal int ExtraWindowBytes;
    internal nint InstanceHandle;
    internal nint IconHandle;
    internal nint CursorHandle;
    internal nint BackgroundBrushHandle;

    internal string MenuName;
    internal string ClassName;

    internal nint SmallIconHandle;
}

[SupportedOSPlatform("Windows")]
[SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time")]
internal static class ShellObjectWatcherNativeMethods
{
    public delegate int WndProcDelegate(nint hwnd, uint msg, nint wparam, nint lparam);

    [DllImport("ole32.dll")]
    public static extern HResult CreateBindCtx(
        int reserved,
        [Out] out IBindCtx bindCtx);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern nint CreateWindowEx(
        int extendedStyle,
        [MarshalAs(UnmanagedType.LPWStr)]
        string className,
        [MarshalAs(UnmanagedType.LPWStr)]
        string windowName,
        int style,
        int x,
        int y,
        int width,
        int height,
        nint parentHandle,
        nint menuHandle,
        nint instanceHandle,
        nint additionalData);

    [DllImport("user32.dll")]
    public static extern int DefWindowProc(
        nint hwnd,
        uint msg,
        nint wparam,
        nint lparam);

    [DllImport("user32.dll")]
    public static extern void DispatchMessage([In] ref Message message);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetMessage(
        [Out] out Message message,
        nint windowHandle,
        uint filterMinMessage,
        uint filterMaxMessage);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern uint RegisterClassEx(
        ref WindowClassEx windowClass);
}
