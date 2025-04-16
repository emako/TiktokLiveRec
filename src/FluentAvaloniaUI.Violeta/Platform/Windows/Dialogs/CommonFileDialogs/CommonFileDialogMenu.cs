using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Dialogs;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.CommonFileDialogs;

[SupportedOSPlatform("Windows")]
public class CommonFileDialogMenu : CommonFileDialogProminentControl
{
    private readonly Collection<CommonFileDialogMenuItem> items = [];

    public CommonFileDialogMenu() : base()
    {
    }

    public CommonFileDialogMenu(string text) : base(text)
    {
    }

    public CommonFileDialogMenu(string name, string text) : base(name, text)
    {
    }

    public Collection<CommonFileDialogMenuItem> Items => items;

    internal override void Attach(IFileDialogCustomize dialog)
    {
        Debug.Assert(dialog != null, "CommonFileDialogMenu.Attach: dialog parameter can not be null");

        dialog!.AddMenu(Id, Text);

        foreach (var item in items)
            dialog.AddControlItem(Id, item.Id, item.Text);

        if (IsProminent)
            dialog.MakeProminent(Id);

        SyncUnmanagedProperties();
    }
}

[SupportedOSPlatform("Windows")]
public class CommonFileDialogMenuItem : CommonFileDialogControl
{
    public CommonFileDialogMenuItem() : base(string.Empty)
    {
    }

    public CommonFileDialogMenuItem(string text) : base(text)
    {
    }

    public event EventHandler Click = delegate { };

    internal override void Attach(IFileDialogCustomize dialog)
    {
    }

    internal void RaiseClickEvent()
    {
        if (Enabled) { Click(this, EventArgs.Empty); }
    }
}
