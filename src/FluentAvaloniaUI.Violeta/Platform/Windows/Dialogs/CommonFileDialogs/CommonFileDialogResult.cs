using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.CommonFileDialogs;

[SupportedOSPlatform("Windows")]
public enum CommonFileDialogResult
{
    None = 0,
    Ok = 1,
    Cancel = 2,
}
