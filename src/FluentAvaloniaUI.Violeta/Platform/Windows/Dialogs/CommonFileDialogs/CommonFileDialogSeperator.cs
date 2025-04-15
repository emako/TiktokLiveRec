using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Dialogs;
using System.Diagnostics;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.CommonFileDialogs;

public class CommonFileDialogSeparator : CommonFileDialogControl
{
    internal override void Attach(IFileDialogCustomize dialog)
    {
        Debug.Assert(dialog != null, "CommonFileDialogSeparator.Attach: dialog parameter can not be null");

        dialog!.AddSeparator(Id);

        SyncUnmanagedProperties();
    }
}
