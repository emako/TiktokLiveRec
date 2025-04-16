using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Dialogs;

[SupportedOSPlatform("Windows")]
public enum DialogShowState
{
    PreShow,
    Showing,
    Closing,
    Closed,
}
