using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.PropertySystem;
using FluentAvalonia.UI.Violeta.Platform.Windows.Natives;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.PropertySystem;

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

[SupportedOSPlatform("Windows")]
public class ShellProperty<T> : IShellProperty
{
    private readonly ShellPropertyDescription description = null!;
    private int? imageReferenceIconIndex;
    private string imageReferencePath = null!;
    private PropertyKey propertyKey;

    internal ShellProperty(
        PropertyKey propertyKey,
        ShellPropertyDescription description,
        ShellObject parent)
    {
        this.propertyKey = propertyKey;
        this.description = description;
        ParentShellObject = parent;
        AllowSetTruncatedValue = false;
    }

    internal ShellProperty(
        PropertyKey propertyKey,
        ShellPropertyDescription description,
        IPropertyStore propertyStore)
    {
        this.propertyKey = propertyKey;
        this.description = description;
        NativePropertyStore = propertyStore;
        AllowSetTruncatedValue = false;
    }

    public bool AllowSetTruncatedValue { get; set; }

    public ShellPropertyDescription Description => description;

    public IconReference IconReference
    {
        get
        {
            if (!CoreHelpers.RunningOnWin7)
            {
                throw new PlatformNotSupportedException(LocalizedMessages.ShellPropertyWindows7);
            }

            GetImageReference();
            var index = imageReferenceIconIndex ?? -1;

            return new IconReference(imageReferencePath, index);
        }
    }

    public PropertyKey PropertyKey => propertyKey;

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    [SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value")]
    public object ValueAsObject
    {
        get
        {
            using var propVar = new PropVariant();
            if (ParentShellObject != null!)
            {
                var store = ShellPropertyCollection.CreateDefaultPropertyStore(ParentShellObject);

                store.GetValue(ref propertyKey, propVar);

                Marshal.ReleaseComObject(store);
                store = null;
            }
            else
            {
                NativePropertyStore?.GetValue(ref propertyKey, propVar);
            }

            return propVar?.Value!;
        }
    }

    public Type ValueType
    {
        get
        {
            Debug.Assert(Description.ValueType == typeof(T));

            return Description.ValueType;
        }
    }

    public string CanonicalName => Description.CanonicalName;

    private IPropertyStore NativePropertyStore { get; set; }
    private ShellObject ParentShellObject { get; set; }

    public void ClearValue()
    {
        using var propVar = new PropVariant();
        StorePropVariantValue(propVar);
    }

    public string FormatForDisplay(PropertyDescriptionFormatOptions format)
    {
        if (Description == null || Description.NativePropertyDescription == null)
        {
            return null!;
        }

        var store = ShellPropertyCollection.CreateDefaultPropertyStore(ParentShellObject);

        using var propVar = new PropVariant();
        store.GetValue(ref propertyKey, propVar);

        Marshal.ReleaseComObject(store);
        store = null;

        var hr = Description.NativePropertyDescription.FormatForDisplay(propVar, ref format, out var formattedString);

        if (!CoreErrorHelper.Succeeded(hr))
            throw new ShellException(hr);

        return formattedString!;
    }

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    private void GetImageReference()
    {
        var store = ShellPropertyCollection.CreateDefaultPropertyStore(ParentShellObject);

        using var propVar = new PropVariant();
        store.GetValue(ref propertyKey, propVar);

        Marshal.ReleaseComObject(store);
        store = null;

        ((IPropertyDescription2)Description.NativePropertyDescription).GetImageReferenceForValue(
            propVar, out var refPath);

        if (refPath == null) { return; }

        var index = ShlwApi.PathParseIconLocation(ref refPath);
        if (refPath != null)
        {
            imageReferencePath = refPath;
            imageReferenceIconIndex = index;
        }
    }

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    private void StorePropVariantValue(PropVariant propVar)
    {
        var guid = new Guid(ShellIIDGuid.IPropertyStore);
        IPropertyStore writablePropStore = null!;
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

            var result = writablePropStore.SetValue(ref propertyKey, propVar);

            if (!AllowSetTruncatedValue && (int)result == ShellNativeMethods.InPlaceStringTruncated)
            {
                throw new ArgumentOutOfRangeException(nameof(propVar), LocalizedMessages.ShellPropertyValueTruncated);
            }

            if (!CoreErrorHelper.Succeeded(result))
            {
                throw new PropertySystemException(LocalizedMessages.ShellPropertySetValue, Marshal.GetExceptionForHR((int)result));
            }

            writablePropStore.Commit();
        }
        catch (InvalidComObjectException e)
        {
            throw new PropertySystemException(LocalizedMessages.ShellPropertyUnableToGetWritableProperty, e);
        }
        catch (InvalidCastException)
        {
            throw new PropertySystemException(LocalizedMessages.ShellPropertyUnableToGetWritableProperty);
        }
        finally
        {
            if (writablePropStore != null)
            {
                Marshal.ReleaseComObject(writablePropStore);
                writablePropStore = null!;
            }
        }
    }
}
