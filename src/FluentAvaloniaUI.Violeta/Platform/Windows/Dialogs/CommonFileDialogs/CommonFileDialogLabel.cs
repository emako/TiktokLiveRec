using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Dialogs;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.CommonFileDialogs;

[SupportedOSPlatform("Windows")]
public class CommonFileDialogLabel : CommonFileDialogControl
{
    public CommonFileDialogLabel()
    {
    }

    public CommonFileDialogLabel(string text) : base(text)
    {
    }

    public CommonFileDialogLabel(string name, string text) : base(name, text)
    {
    }

    internal override void Attach(IFileDialogCustomize dialog)
    {
        Debug.Assert(dialog != null, "CommonFileDialog.Attach: dialog parameter can not be null");

        dialog!.AddText(Id, Text);

        SyncUnmanagedProperties();
    }
}
