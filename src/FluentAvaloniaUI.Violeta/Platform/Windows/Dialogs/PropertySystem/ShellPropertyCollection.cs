using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.PropertySystem;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.PropertySystem;

#pragma warning disable CS8618

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
[SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value")]
public class ShellPropertyCollection : ReadOnlyCollection<IShellProperty>, IDisposable
{
    public ShellPropertyCollection(ShellObject parent)
        : base([])
    {
        ParentShellObject = parent;
        IPropertyStore nativePropertyStore = null!;
        try
        {
            nativePropertyStore = CreateDefaultPropertyStore(ParentShellObject);
        }
        catch
        {
            if (parent != null!)
            {
                parent.Dispose();
            }
            throw;
        }
        finally
        {
            if (nativePropertyStore != null)
            {
                Marshal.ReleaseComObject(nativePropertyStore);
                nativePropertyStore = null!;
            }
        }
    }

    public ShellPropertyCollection(string path) : this(ShellObjectFactory.Create(path))
    {
    }

    internal ShellPropertyCollection(IPropertyStore nativePropertyStore)
        : base([])
    {
        NativePropertyStore = nativePropertyStore;
    }

    ~ShellPropertyCollection()
    {
        Dispose(false);
    }

    private IPropertyStore NativePropertyStore { get; set; }
    private ShellObject ParentShellObject { get; set; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    internal static IPropertyStore CreateDefaultPropertyStore(ShellObject shellObj)
    {
        var guid = new Guid(ShellIIDGuid.IPropertyStore);
        var hr = shellObj.NativeShellItem2.GetPropertyStore(
               GetPropertyStoreOptions.BestEffort,
               ref guid,
               out var nativePropertyStore);

        if (nativePropertyStore == null || !CoreErrorHelper.Succeeded(hr))
        {
            throw new ShellException(hr);
        }

        return nativePropertyStore;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (NativePropertyStore != null)
        {
            Marshal.ReleaseComObject(NativePropertyStore);
            NativePropertyStore = null!;
        }
    }
}
