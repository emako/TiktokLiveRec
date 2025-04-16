using FluentAvalonia.UI.Violeta.Platform.Windows.Natives;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Versioning;
using System.Text;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop;

[SupportedOSPlatform("Windows")]
[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public static class CoreHelpers
{
    public static bool RunningOnVista => Environment.OSVersion.Version.Major >= 6;

    public static bool RunningOnWin7 =>
            // Verifies that OS version is 6.1 or greater, and the Platform is WinNT.
            Environment.OSVersion.Platform == PlatformID.Win32NT &&
                Environment.OSVersion.Version.CompareTo(new Version(6, 1)) >= 0;

    public static bool RunningOnXP => Environment.OSVersion.Platform == PlatformID.Win32NT &&
                Environment.OSVersion.Version.Major >= 5;

    public static string GetStringResource(string resourceId)
    {
        string[] parts;
        string library;
        int index;

        if (string.IsNullOrEmpty(resourceId)) { return string.Empty; }

        resourceId = resourceId.Replace("shell32,dll", "shell32.dll");
        parts = resourceId.Split([',']);

        library = parts[0];
        library = library.Replace(@"@", string.Empty);
        library = Environment.ExpandEnvironmentVariables(library);
        var handle = Kernel32.LoadLibrary(library);

        parts[1] = parts[1].Replace("-", string.Empty);
        index = int.Parse(parts[1], CultureInfo.InvariantCulture);

        var stringValue = new StringBuilder(255);
        var retval = User32.LoadString(handle, index, stringValue, 255);

        return retval != 0 ? stringValue.ToString() : null!;
    }

    public static void ThrowIfNotVista()
    {
        if (!RunningOnVista)
        {
            throw new PlatformNotSupportedException("CoreHelpersRunningOnVista");
        }
    }

    public static void ThrowIfNotWin7()
    {
        if (!RunningOnWin7)
        {
            throw new PlatformNotSupportedException("CoreHelpersRunningOn7");
        }
    }

    public static void ThrowIfNotXP()
    {
        if (!RunningOnXP)
        {
            throw new PlatformNotSupportedException("CoreHelpersRunningOnXp");
        }
    }
}
