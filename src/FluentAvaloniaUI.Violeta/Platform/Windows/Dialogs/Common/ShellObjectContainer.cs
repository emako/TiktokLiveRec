using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;

#pragma warning disable CS8618

[SupportedOSPlatform("Windows")]
[SuppressMessage("Microsoft.Naming", "CA1710:")]
public abstract class ShellContainer : ShellObject, IEnumerable<ShellObject>, IDisposable
{
    private IShellFolder desktopFolderEnumeration;
    private IShellFolder nativeShellFolder;

    internal ShellContainer()
    {
    }

    internal ShellContainer(IShellItem2 shellItem) : base(shellItem)
    {
    }

    internal IShellFolder NativeShellFolder
    {
        get
        {
            if (nativeShellFolder == null)
            {
                var guid = new Guid(ShellIIDGuid.IShellFolder);
                var handler = new Guid(ShellBHIDGuid.ShellFolderObject);

                var hr = NativeShellItem.BindToHandler(
                    0, ref handler, ref guid, out nativeShellFolder);

                if (CoreErrorHelper.Failed(hr))
                {
                    var str = ShellHelper.GetParsingName(NativeShellItem);
                    if (str != null && str != Environment.GetFolderPath(Environment.SpecialFolder.Desktop))
                    {
                        throw new ShellException(hr);
                    }
                }
            }

            return nativeShellFolder;
        }
    }

    public IEnumerator<ShellObject> GetEnumerator()
    {
        if (NativeShellFolder == null)
        {
            if (desktopFolderEnumeration == null)
            {
                _ = Shell32.SHGetDesktopFolder(out desktopFolderEnumeration);
            }

            nativeShellFolder = desktopFolderEnumeration;
        }

        return new ShellFolderItems(this);
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => new ShellFolderItems(this);

    protected override void Dispose(bool disposing)
    {
        if (nativeShellFolder != null)
        {
            Marshal.ReleaseComObject(nativeShellFolder);
            nativeShellFolder = null!;
        }

        if (desktopFolderEnumeration != null)
        {
            Marshal.ReleaseComObject(desktopFolderEnumeration);
            desktopFolderEnumeration = null!;
        }

        base.Dispose(disposing);
    }
}
