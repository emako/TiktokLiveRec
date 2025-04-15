using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.PropertySystem;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.PropertySystem;

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword

[ComImport]
[Guid(ShellIIDGuid.IPropertyDescription)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IPropertyDescription
{
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetPropertyKey(out PropertyKey pkey);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetCanonicalName([MarshalAs(UnmanagedType.LPWStr)] out string ppszName);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetPropertyType(out VarEnum pvartype);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetDisplayName(out nint ppszName);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetEditInvitation(out nint ppszInvite);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetTypeFlags([In] PropertyTypeOptions mask, out PropertyTypeOptions ppdtFlags);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetViewFlags(out PropertyViewOptions ppdvFlags);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetDefaultColumnWidth(out uint pcxChars);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetDisplayType(out PropertyDisplayType pdisplaytype);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetColumnState(out PropertyColumnStateOptions pcsFlags);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetGroupingRange(out PropertyGroupingRange pgr);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetRelativeDescriptionType(out PropertySystemNativeMethods.RelativeDescriptionType prdt);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetRelativeDescription([In] PropVariant propvar1, [In] PropVariant propvar2, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDesc1, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDesc2);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetSortDescription(out PropertySortDescription psd);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetSortDescriptionLabel([In] bool fDescending, out nint ppszDescription);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetAggregationType(out PropertyAggregationType paggtype);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetConditionType(out PropertyConditionType pcontype, out PropertyConditionOperation popDefault);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetEnumTypeList([In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out IPropertyEnumTypeList ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void CoerceToCanonicalValue([In, Out] PropVariant propvar);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult FormatForDisplay([In] PropVariant propvar, [In] ref PropertyDescriptionFormatOptions pdfFlags, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplay);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult IsValueCanonical([In] PropVariant propvar);
}

[ComImport]
[Guid(ShellIIDGuid.IPropertyDescription2)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
public interface IPropertyDescription2 : IPropertyDescription
{
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetPropertyKey(out PropertyKey pkey);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetCanonicalName([MarshalAs(UnmanagedType.LPWStr)] out string ppszName);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetPropertyType(out VarEnum pvartype);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetDisplayName([MarshalAs(UnmanagedType.LPWStr)] out string ppszName);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetEditInvitation([MarshalAs(UnmanagedType.LPWStr)] out string ppszInvite);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetTypeFlags([In] PropertyTypeOptions mask, out PropertyTypeOptions ppdtFlags);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetViewFlags(out PropertyViewOptions ppdvFlags);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetDefaultColumnWidth(out uint pcxChars);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetDisplayType(out PropertyDisplayType pdisplaytype);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetColumnState(out uint pcsFlags);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetGroupingRange(out PropertyGroupingRange pgr);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetRelativeDescriptionType(out PropertySystemNativeMethods.RelativeDescriptionType prdt);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetRelativeDescription(
       [In] PropVariant propvar1,
       [In] PropVariant propvar2,
       [MarshalAs(UnmanagedType.LPWStr)] out string ppszDesc1,
       [MarshalAs(UnmanagedType.LPWStr)] out string ppszDesc2);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetSortDescription(out PropertySortDescription psd);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetSortDescriptionLabel([In] int fDescending, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDescription);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetAggregationType(out PropertyAggregationType paggtype);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetConditionType(
        out PropertyConditionType pcontype,
        out PropertyConditionOperation popDefault);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetEnumTypeList([In] ref Guid riid, out nint ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void CoerceToCanonicalValue([In, Out] PropVariant ppropvar);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void FormatForDisplay([In] PropVariant propvar, [In] ref PropertyDescriptionFormatOptions pdfFlags, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplay);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult IsValueCanonical([In] PropVariant propvar);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetImageReferenceForValue(
        [In] PropVariant propvar,
        [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszImageRes);
}

[ComImport]
[Guid(ShellIIDGuid.IPropertyDescriptionList)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IPropertyDescriptionList
{
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetCount(out uint pcElem);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetAt([In] uint iElem, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IPropertyDescription ppv);
}

[ComImport]
[Guid(ShellIIDGuid.IPropertyEnumType)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
public interface IPropertyEnumType
{
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetEnumType([Out] out PropEnumType penumtype);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetValue([Out] PropVariant ppropvar);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetRangeMinValue([Out] PropVariant ppropvar);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetRangeSetValue([Out] PropVariant ppropvar);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetDisplayText([Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplay);
}

[ComImport]
[Guid(ShellIIDGuid.IPropertyEnumType2)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
public interface IPropertyEnumType2 : IPropertyEnumType
{
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetEnumType([Out] out PropEnumType penumtype);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetValue([Out] PropVariant ppropvar);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetRangeMinValue([Out] PropVariant ppropvar);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetRangeSetValue([Out] PropVariant ppropvar);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetDisplayText([Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplay);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetImageReference([Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszImageRes);
}

[ComImport]
[Guid(ShellIIDGuid.IPropertyEnumTypeList)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IPropertyEnumTypeList
{
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetCount([Out] out uint pctypes);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetAt(
        [In] uint itype,
        [In] ref Guid riid,
        [Out, MarshalAs(UnmanagedType.Interface)] out IPropertyEnumType ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetConditionAt(
        [In] uint index,
        [In] ref Guid riid,
        out nint ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void FindMatchingIndex(
        [In] PropVariant propvarCmp,
        [Out] out uint pnIndex);
}

[ComImport]
[Guid(ShellIIDGuid.IPropertyStore)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
public interface IPropertyStore
{
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetCount([Out] out uint propertyCount);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetAt([In] uint propertyIndex, out PropertyKey key);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetValue([In] ref PropertyKey key, [Out] PropVariant pv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), PreserveSig]
    public HResult SetValue([In] ref PropertyKey key, [In] PropVariant pv);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult Commit();
}

[ComImport]
[Guid(ShellIIDGuid.IPropertyStoreCache)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
public interface IPropertyStoreCache
{
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetState(ref PropertyKey key, [Out] out PropertyStoreCacheState state);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetValueAndState(ref PropertyKey propKey, [Out] PropVariant pv, [Out] out PropertyStoreCacheState state);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult SetState(ref PropertyKey propKey, PropertyStoreCacheState state);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult SetValueAndState(ref PropertyKey propKey, [In] PropVariant pv, PropertyStoreCacheState state);
}

[ComImport]
[Guid(ShellIIDGuid.IPropertyStoreCapabilities)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
public interface IPropertyStoreCapabilities
{
    public HResult IsPropertyWritable([In] ref PropertyKey propertyKey);
}
