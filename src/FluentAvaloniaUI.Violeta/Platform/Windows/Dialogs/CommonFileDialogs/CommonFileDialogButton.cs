using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Dialogs;
using System.Diagnostics;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.CommonFileDialogs;

public class CommonFileDialogButton : CommonFileDialogProminentControl
{
    public CommonFileDialogButton() : base(string.Empty)
    {
    }

    public CommonFileDialogButton(string text) : base(text)
    {
    }

    public CommonFileDialogButton(string name, string text) : base(name, text)
    {
    }

    public event EventHandler Click = delegate { };

    internal override void Attach(IFileDialogCustomize dialog)
    {
        Debug.Assert(dialog != null, "CommonFileDialogButton.Attach: dialog parameter can not be null");

        dialog!.AddPushButton(Id, Text);

        if (IsProminent) { dialog.MakeProminent(Id); }

        SyncUnmanagedProperties();
    }

    internal void RaiseClickEvent()
    {
        if (Enabled) { Click(this, EventArgs.Empty); }
    }
}
