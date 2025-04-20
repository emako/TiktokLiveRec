using FluentAvalonia.UI.Violeta.Platform.Windows.Natives;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace FluentAvalonia.UI.Controls;

public class MessageBoxButtonText
{
    public string Ok { get; } = GetString(MessageBoxResult.Ok);
    public string Yes { get; } = GetString(MessageBoxResult.Yes);
    public string No { get; } = GetString(MessageBoxResult.No);
    public string Abort { get; } = GetString(MessageBoxResult.Abort);
    public string Cancel { get; } = GetString(MessageBoxResult.Cancel);

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    [SuppressMessage("Performance", "SYSLIB1045:Convert to 'GeneratedRegexAttribute'.")]
    private static string GetString(MessageBoxResult button)
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            return GetString(button switch
            {
                MessageBoxResult.Ok => User32.DialogBoxCommand.IDOK,
                MessageBoxResult.Yes => User32.DialogBoxCommand.IDYES,
                MessageBoxResult.No => User32.DialogBoxCommand.IDNO,
                MessageBoxResult.Abort => User32.DialogBoxCommand.IDABORT,
                MessageBoxResult.Cancel => User32.DialogBoxCommand.IDCANCEL,
                _ => User32.DialogBoxCommand.IDIGNORE,
            });

            static string GetString(User32.DialogBoxCommand wBtn)
            {
                nint strPtr = User32.MB_GetString((uint)wBtn);
                string src = Marshal.PtrToStringAuto(strPtr)?.TrimStart('&')!;
                return new Regex(@"\([^)]*\)").Replace(src, string.Empty).Replace("&", string.Empty);
            }
        }

        return button.ToString();
    }
}
