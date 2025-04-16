using FluentAvalonia.UI.Violeta.Platform.Windows.Natives;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

namespace FluentAvaloniaUI.Violeta.Platform.Windows;

[SupportedOSPlatform("Windows")]
[SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time")]
public static class WindowsKeepAwake
{
    public static void SetKeepAwake(bool isOn = true)
    {
        if (isOn)
        {
            // Start keep awake
            _ = Kernel32.SetThreadExecutionState(Kernel32.EXECUTION_STATE.ES_CONTINUOUS | Kernel32.EXECUTION_STATE.ES_SYSTEM_REQUIRED | Kernel32.EXECUTION_STATE.ES_AWAYMODE_REQUIRED);
        }
        else
        {
            // Stop keep awake
            _ = Kernel32.SetThreadExecutionState(Kernel32.EXECUTION_STATE.ES_CONTINUOUS);
        }
    }
}
