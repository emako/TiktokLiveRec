using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.PropertySystem;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8618

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.PropertySystem;

[SupportedOSPlatform("Windows")]
public class ShellPropertyWriter : IDisposable
{
    internal IPropertyStore writablePropStore;

    private ShellObject parentShellObject;

    internal ShellPropertyWriter(ShellObject parent)
    {
        ParentShellObject = parent;

        var guid = new Guid(ShellIIDGuid.IPropertyStore);

        try
        {
            var hr = ParentShellObject.NativeShellItem2.GetPropertyStore(
                    GetPropertyStoreOptions.ReadWrite,
                    ref guid,
                    out writablePropStore);

            if (!CoreErrorHelper.Succeeded(hr))
            {
                throw new PropertySystemException(LocalizedMessages.ShellPropertyUnableToGetWritableProperty,
                    Marshal.GetExceptionForHR(hr));
            }
            else
            {
                if (ParentShellObject.NativePropertyStore == null)
                {
                    ParentShellObject.NativePropertyStore = writablePropStore;
                }
            }
        }
        catch (InvalidComObjectException e)
        {
            throw new PropertySystemException(LocalizedMessages.ShellPropertyUnableToGetWritableProperty, e);
        }
        catch (InvalidCastException)
        {
            throw new PropertySystemException(LocalizedMessages.ShellPropertyUnableToGetWritableProperty);
        }
    }

    ~ShellPropertyWriter()
    {
        Dispose(false);
    }

    protected ShellObject ParentShellObject
    {
        get => parentShellObject;
        private set => parentShellObject = value;
    }

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    public void Close()
    {
        if (writablePropStore != null)
        {
            writablePropStore.Commit();

            Marshal.ReleaseComObject(writablePropStore);
            writablePropStore = null!;
        }

        ParentShellObject.NativePropertyStore = null!;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void WriteProperty(PropertyKey key, object value) => WriteProperty(key, value, true);

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    public void WriteProperty(PropertyKey key, object value, bool allowTruncatedValue)
    {
        if (writablePropStore == null)
            throw new InvalidOperationException("Writeable store has been closed.");

        using var propVar = PropVariant.FromObject(value);
        var result = writablePropStore.SetValue(ref key, propVar);

        if (!allowTruncatedValue && ((int)result == ShellNativeMethods.InPlaceStringTruncated))
        {
            Marshal.ReleaseComObject(writablePropStore);
            writablePropStore = null!;

            throw new ArgumentOutOfRangeException(nameof(value), LocalizedMessages.ShellPropertyValueTruncated);
        }

        if (!CoreErrorHelper.Succeeded(result))
        {
            throw new PropertySystemException(LocalizedMessages.ShellPropertySetValue, Marshal.GetExceptionForHR((int)result));
        }
    }

    public void WriteProperty(string canonicalName, object value) => WriteProperty(canonicalName, value, true);

    public void WriteProperty(string canonicalName, object value, bool allowTruncatedValue)
    {
        var result = PropertySystemNativeMethods.PSGetPropertyKeyFromName(canonicalName, out var propKey);

        if (!CoreErrorHelper.Succeeded(result))
        {
            throw new ArgumentException(
                LocalizedMessages.ShellInvalidCanonicalName,
                Marshal.GetExceptionForHR(result));
        }

        WriteProperty(propKey, value, allowTruncatedValue);
    }

    public void WriteProperty(IShellProperty shellProperty, object value) => WriteProperty(shellProperty, value, true);

    public void WriteProperty(IShellProperty shellProperty, object value, bool allowTruncatedValue)
    {
        ArgumentNullException.ThrowIfNull(shellProperty);
        WriteProperty(shellProperty.PropertyKey, value, allowTruncatedValue);
    }

    public void WriteProperty<T>(ShellProperty<T> shellProperty, T value) => WriteProperty<T>(shellProperty, value, true);

    public void WriteProperty<T>(ShellProperty<T> shellProperty, T value, bool allowTruncatedValue)
    {
        ArgumentNullException.ThrowIfNull(shellProperty);
        WriteProperty(shellProperty.PropertyKey, value!, allowTruncatedValue);
    }

    protected virtual void Dispose(bool disposing) => Close();
}
