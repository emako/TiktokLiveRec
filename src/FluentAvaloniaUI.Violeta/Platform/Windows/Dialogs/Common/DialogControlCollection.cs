using System.Collections.ObjectModel;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;

[SupportedOSPlatform("Windows")]
public sealed class DialogControlCollection<T> : Collection<T> where T : DialogControl
{
    private readonly IDialogControlHost hostingDialog;

    internal DialogControlCollection(IDialogControlHost host) => hostingDialog = host;

    public T this[string name]
    {
        get
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(LocalizedMessages.DialogCollectionControlNameNull, nameof(name));
            }

            return Items.FirstOrDefault(x => x.Name == name)!;
        }
    }

    internal DialogControl GetControlbyId(int id) => Items.FirstOrDefault(x => x.Id == id)!;

    protected override void InsertItem(int index, T control)
    {
        if (Items.Contains(control))
        {
            throw new InvalidOperationException(LocalizedMessages.DialogCollectionCannotHaveDuplicateNames);
        }
        if (control.HostingDialog != null)
        {
            throw new InvalidOperationException(LocalizedMessages.DialogCollectionControlAlreadyHosted);
        }
        if (!hostingDialog.IsCollectionChangeAllowed())
        {
            throw new InvalidOperationException(LocalizedMessages.DialogCollectionModifyShowingDialog);
        }

        control.HostingDialog = hostingDialog;
        base.InsertItem(index, control);

        hostingDialog.ApplyCollectionChanged();
    }

    protected override void RemoveItem(int index)
    {
        if (!hostingDialog.IsCollectionChangeAllowed())
        {
            throw new InvalidOperationException(LocalizedMessages.DialogCollectionModifyShowingDialog);
        }

        var control = (DialogControl)Items[index];

        control.HostingDialog = null!;
        base.RemoveItem(index);

        hostingDialog.ApplyCollectionChanged();
    }
}
