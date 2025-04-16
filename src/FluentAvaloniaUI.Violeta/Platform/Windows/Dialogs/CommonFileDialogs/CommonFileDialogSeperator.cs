using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Dialogs;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.CommonFileDialogs;

[SupportedOSPlatform("Windows")]
public class CommonFileDialogSeparator : CommonFileDialogControl
{
    internal override void Attach(IFileDialogCustomize dialog)
    {
        Debug.Assert(dialog != null, "CommonFileDialogSeparator.Attach: dialog parameter can not be null");

        dialog!.AddSeparator(Id);

        SyncUnmanagedProperties();
    }
}
