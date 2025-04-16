using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Dialogs;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Versioning;

#pragma warning disable CS8618

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.CommonFileDialogs;

[SupportedOSPlatform("Windows")]
public class CommonFileDialogGroupBox : CommonFileDialogProminentControl
{
    private Collection<DialogControl> items;

    public CommonFileDialogGroupBox()
        : base(string.Empty) => Initialize();

    public CommonFileDialogGroupBox(string text)
        : base(text) => Initialize();

    public CommonFileDialogGroupBox(string name, string text)
        : base(name, text) => Initialize();

    public Collection<DialogControl> Items => items;

    internal override void Attach(IFileDialogCustomize dialog)
    {
        Debug.Assert(dialog != null, "CommonFileDialogGroupBox.Attach: dialog parameter can not be null");

        dialog!.StartVisualGroup(Id, Text);

        foreach (CommonFileDialogControl item in items)
        {
            item.HostingDialog = HostingDialog;
            item.Attach(dialog);
        }

        dialog.EndVisualGroup();

        if (IsProminent)
            dialog.MakeProminent(Id);

        SyncUnmanagedProperties();
    }

    private void Initialize() => items = new Collection<DialogControl>();
}
