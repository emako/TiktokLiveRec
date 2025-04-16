using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Dialogs;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

#pragma warning disable CS8618

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.CommonFileDialogs;

[SupportedOSPlatform("Windows")]
public sealed class CommonOpenFileDialog : CommonFileDialog
{
    private bool allowNonFileSystem;
    private bool isFolderPicker;
    private bool multiselect;
    private NativeFileOpenDialog openDialogCoClass;

    public CommonOpenFileDialog() : base() => EnsureReadOnly = true;

    public CommonOpenFileDialog(string name) : base(name) => EnsureReadOnly = true;

    public bool AllowNonFileSystemItems
    {
        get => allowNonFileSystem;
        set => allowNonFileSystem = value;
    }

    public IEnumerable<string> FileNames
    {
        get
        {
            CheckFileNamesAvailable();
            return base.FileNameCollection;
        }
    }

    public ICollection<ShellObject> FilesAsShellObject
    {
        get
        {
            CheckFileItemsAvailable();

            ICollection<ShellObject> resultItems = [];

            foreach (var si in items)
            {
                resultItems.Add(ShellObjectFactory.Create(si));
            }

            return resultItems;
        }
    }

    public bool IsFolderPicker
    {
        get => isFolderPicker;
        set => isFolderPicker = value;
    }

    public bool Multiselect
    {
        get => multiselect;
        set => multiselect = value;
    }

    internal override void CleanUpNativeFileDialog()
    {
        if (openDialogCoClass != null)
        {
            Marshal.ReleaseComObject(openDialogCoClass);
        }
    }

    internal override FileOpenOptions GetDerivedOptionFlags(FileOpenOptions flags)
    {
        if (multiselect)
        {
            flags |= FileOpenOptions.AllowMultiSelect;
        }
        if (isFolderPicker)
        {
            flags |= FileOpenOptions.PickFolders;
        }

        if (!allowNonFileSystem)
        {
            flags |= FileOpenOptions.ForceFilesystem;
        }
        else if (allowNonFileSystem)
        {
            flags |= FileOpenOptions.AllNonStorageItems;
        }

        return flags;
    }

    internal override IFileDialog GetNativeFileDialog()
    {
        Debug.Assert(openDialogCoClass != null, "Must call Initialize() before fetching dialog interface");

        return openDialogCoClass!;
    }

    internal override void InitializeNativeFileDialog()
    {
        openDialogCoClass ??= new NativeFileOpenDialog();
    }

    internal override void PopulateWithFileNames(Collection<string> names)
    {
        openDialogCoClass.GetResults(out var resultsArray);
        resultsArray.GetCount(out var count);
        names.Clear();
        for (var i = 0; i < count; i++)
        {
            names.Add(GetFileNameFromShellItem(GetShellItemAt(resultsArray, i)));
        }
    }

    internal override void PopulateWithIShellItems(Collection<IShellItem> items)
    {
        openDialogCoClass.GetResults(out var resultsArray);
        resultsArray.GetCount(out var count);
        items.Clear();
        for (var i = 0; i < count; i++)
        {
            items.Add(GetShellItemAt(resultsArray, i));
        }
    }
}
