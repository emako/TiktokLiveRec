using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;

#pragma warning disable CS8618

[SupportedOSPlatform("Windows")]
internal class ShellFolderItems : IEnumerator<ShellObject>
{
    private readonly ShellContainer nativeShellFolder;
    private ShellObject currentItem;
    private IEnumIDList nativeEnumIdList;

    internal ShellFolderItems(ShellContainer nativeShellFolder)
    {
        this.nativeShellFolder = nativeShellFolder;

        var hr = nativeShellFolder.NativeShellFolder.EnumObjects(
            0,
            ShellFolderEnumerationOptions.Folders | ShellFolderEnumerationOptions.NonFolders,
            out nativeEnumIdList);

        if (!CoreErrorHelper.Succeeded(hr))
        {
            if (hr == HResult.Canceled)
            {
                throw new System.IO.FileNotFoundException();
            }
            else
            {
                throw new ShellException(hr);
            }
        }
    }

    public ShellObject Current => currentItem;

    object IEnumerator.Current => currentItem;

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    public void Dispose()
    {
        if (nativeEnumIdList != null)
        {
            Marshal.ReleaseComObject(nativeEnumIdList);
            nativeEnumIdList = null!;
        }
    }

    public bool MoveNext()
    {
        if (nativeEnumIdList == null) { return false; }

        uint itemsRequested = 1;
        var hr = nativeEnumIdList.Next(itemsRequested, out var item, out var numItemsReturned);

        if (numItemsReturned < itemsRequested || hr != HResult.Ok) { return false; }

        currentItem = ShellObjectFactory.Create(item, nativeShellFolder);

        return true;
    }

    public void Reset()
    {
        nativeEnumIdList?.Reset();
    }
}
