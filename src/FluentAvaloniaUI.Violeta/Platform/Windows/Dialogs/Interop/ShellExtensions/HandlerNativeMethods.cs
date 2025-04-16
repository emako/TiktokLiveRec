using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.ShellExtensions;

[SupportedOSPlatform("Windows")]
[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("b7d14566-0509-4cce-a71f-0a554233bd9b")]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
public interface IInitializeWithFile
{
    public void Initialize([MarshalAs(UnmanagedType.LPWStr)] string filePath, AccessModes fileMode);
}

[SupportedOSPlatform("Windows")]
[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("7f73be3f-fb79-493c-a6c7-7ee14e245841")]
public interface IInitializeWithItem
{
    public void Initialize([In, MarshalAs(UnmanagedType.IUnknown)] object shellItem, AccessModes accessMode);
}

[SupportedOSPlatform("Windows")]
[SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
[ComVisible(true)]
[Guid("b824b49d-22ac-4161-ac8a-9916e8fa3f7f")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IInitializeWithStream
{
    public void Initialize(IStream stream, AccessModes fileMode);
}

[SupportedOSPlatform("Windows")]
[ComVisible(true)]
[Guid("e357fccd-a995-4576-b01f-234630154e96")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IThumbnailProvider
{
    [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
    public void GetThumbnail(uint squareLength, out nint bitmapHandle, out uint bitmapType);
}

[SupportedOSPlatform("Windows")]
[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("fc4801a3-2ba9-11cf-a229-00aa003d7352")]
internal interface IObjectWithSite
{
    public void SetSite([In, MarshalAs(UnmanagedType.IUnknown)] object pUnkSite);

    public void GetSite(ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppvSite);
}

[SupportedOSPlatform("Windows")]
[ComImport]
[Guid("00000114-0000-0000-C000-000000000046")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
internal interface IOleWindow
{
    public void GetWindow(out nint phwnd);

    public void ContextSensitiveHelp([MarshalAs(UnmanagedType.Bool)] bool fEnterMode);
}

[SupportedOSPlatform("Windows")]
[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("8895b1c6-b41f-4c1c-a562-0d564250836f")]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
internal interface IPreviewHandler
{
    public void SetWindow(nint hwnd, ref RECT rect);

    public void SetRect(ref RECT rect);

    public void DoPreview();

    public void Unload();

    public void SetFocus();

    public void QueryFocus(out nint phwnd);

    [PreserveSig]
    public HResult TranslateAccelerator(ref MSG pmsg);
}

[SupportedOSPlatform("Windows")]
[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("fec87aaf-35f9-447a-adb7-20234491401a")]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
internal interface IPreviewHandlerFrame
{
    public void GetWindowContext(nint pinfo);

    [PreserveSig]
    public HResult TranslateAccelerator(ref MSG pmsg);
};

[SupportedOSPlatform("Windows")]
[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("8327b13c-b63f-4b24-9b8a-d010dcc3f599")]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
internal interface IPreviewHandlerVisuals
{
    public void SetBackgroundColor(COLORREF color);

    public void SetFont(ref LogFont plf);

    public void SetTextColor(COLORREF color);
}

[SupportedOSPlatform("Windows")]
[StructLayout(LayoutKind.Sequential)]
internal struct COLORREF
{
    public uint Dword;

    [SuppressMessage("Style", "IDE0251:Make member 'readonly'")]
    public Color Color => Color.FromArgb(
                (int)(0x000000FFU & Dword),
                (int)(0x0000FF00U & Dword) >> 8,
                (int)(0x00FF0000U & Dword) >> 16);
}

[SupportedOSPlatform("Windows")]
[StructLayout(LayoutKind.Sequential)]
internal struct MSG
{
    public nint hwnd;
    public int message;
    public nint wParam;
    public nint lParam;
    public int time;
    public int pt_x;
    public int pt_y;
}

[SupportedOSPlatform("Windows")]
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
internal class LogFont
{
    internal int height;
    internal int width;
    internal int escapement;
    internal int orientation;
    internal int weight;
    internal byte italic;
    internal byte underline;
    internal byte strikeOut;
    internal byte charSet;
    internal byte outPrecision;
    internal byte clipPrecision;
    internal byte quality;
    internal byte pitchAndFamily;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    internal string lfFaceName = string.Empty;
}

[SupportedOSPlatform("Windows")]
[StructLayout(LayoutKind.Sequential)]
[SuppressMessage("Style", "IDE0251:Make member 'readonly'")]
public struct POINT(int x, int y) : IEquatable<POINT>
{
    public int X = x;
    public int Y = y;

    public bool Equals(POINT other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        return obj is POINT other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public static bool operator ==(POINT left, POINT right) => left.Equals(right);

    public static bool operator !=(POINT left, POINT right) => !(left == right);
}

[SupportedOSPlatform("Windows")]
[StructLayout(LayoutKind.Sequential)]
[SuppressMessage("Style", "IDE0250:Make struct 'readonly'")]
public struct RECT
{
    public readonly int Left;
    public readonly int Top;
    public readonly int Right;
    public readonly int Bottom;
}

[SupportedOSPlatform("Windows")]
[StructLayout(LayoutKind.Sequential)]
public struct SIZE()
{
    public double Width;
    public double Height;

    public SIZE(double width, double height) : this()
    {
        Width = width;
        Height = height;
    }
}

[SupportedOSPlatform("Windows")]
public struct MESSAGE
{
    public nint HWnd { readonly get; set; }

    public int Msg { readonly get; set; }

    public nint WParam { readonly get; set; }

    public nint LParam { readonly get; set; }

    public nint Result { readonly get; set; }
}
