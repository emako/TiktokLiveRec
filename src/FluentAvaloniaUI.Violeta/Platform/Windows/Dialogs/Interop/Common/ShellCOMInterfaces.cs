using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.PropertySystem;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.ShellExtensions;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Taskbar;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;

#pragma warning disable CS0108

public enum SICHINTF
{
    SICHINT_DISPLAY = 0x00000000,
    SICHINT_CANONICAL = 0x10000000,
    SICHINT_TEST_FILESYSPATH_IF_NOT_EQUAL = 0x20000000,
    SICHINT_ALLFIELDS = unchecked((int)0x80000000)
}

[ComImport()]
[Guid(ShellIIDGuid.ICondition)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
public interface ICondition : IPersistStream
{
    [PreserveSig]
    public void GetClassID(out Guid pClassID);

    [PreserveSig]
    public HResult IsDirty();

    [PreserveSig]
    public HResult Load([In, MarshalAs(UnmanagedType.Interface)] IStream stm);

    [PreserveSig]
    public HResult Save([In, MarshalAs(UnmanagedType.Interface)] IStream stm, bool fRemember);

    [PreserveSig]
    public HResult GetSizeMax(out ulong cbSize);

    [PreserveSig]
    public HResult GetConditionType([Out()] out SearchConditionType pNodeType);

    [PreserveSig]
    public HResult GetSubConditions([In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out object ppv);

    [PreserveSig]
    public HResult GetComparisonInfo(
        [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszPropertyName,
        [Out] out SearchConditionOperation pcop,
        [Out] PropVariant ppropvar);

    [PreserveSig]
    public HResult GetValueType([Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszValueTypeName);

    [PreserveSig]
    public HResult GetValueNormalization([Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszNormalization);

    [PreserveSig]
    public HResult GetInputTerms([Out] out IRichChunk ppPropertyTerm, [Out] out IRichChunk ppOperationTerm, [Out] out IRichChunk ppValueTerm);

    [PreserveSig]
    public HResult Clone([Out()] out ICondition ppc);
};

[ComImport]
[Guid(ShellIIDGuid.IConditionFactory)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
public interface IConditionFactory
{
    [PreserveSig]
    public HResult MakeNot([In] ICondition pcSub, [In] bool fSimplify, [Out] out ICondition ppcResult);

    [PreserveSig]
    public HResult MakeAndOr([In] SearchConditionType ct, [In] IEnumUnknown peuSubs, [In] bool fSimplify, [Out] out ICondition ppcResult);

    [PreserveSig]
    public HResult MakeLeaf(
        [In, MarshalAs(UnmanagedType.LPWStr)] string pszPropertyName,
        [In] SearchConditionOperation cop,
        [In, MarshalAs(UnmanagedType.LPWStr)] string pszValueType,
        [In] PropVariant ppropvar,
        IRichChunk richChunk1,
        IRichChunk richChunk2,
        IRichChunk richChunk3,
        [In] bool fExpand,
        [Out] out ICondition ppcResult);

    [PreserveSig]
    public HResult Resolve();
};

[ComImport]
[Guid("24264891-E80B-4fd3-B7CE-4FF2FAE8931F")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
public interface IEntity;

[ComImport]
[Guid(ShellIIDGuid.IEnumIDList)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IEnumIDList
{
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult Next(uint celt, out nint rgelt, out uint pceltFetched);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult Skip([In] uint celt);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult Reset();

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult Clone([MarshalAs(UnmanagedType.Interface)] out IEnumIDList ppenum);
}

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid(ShellIIDGuid.IEnumUnknown)]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
public interface IEnumUnknown
{
    [PreserveSig]
    public HResult Next(uint requestedNumber, ref nint buffer, ref uint fetchedNumber);

    [PreserveSig]
    public HResult Skip(uint number);

    [PreserveSig]
    public HResult Reset();

    [PreserveSig]
    public HResult Clone(out IEnumUnknown result);
}

[ComImport]
[Guid(ShellIIDGuid.IModalWindow)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
public interface IModalWindow
{
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
    PreserveSig]
    public int Show([In] nint parent);
}

[ComImport]
[Guid(ShellIIDGuid.IConditionFactory)]
[CoClass(typeof(ConditionFactoryCoClass))]
public interface INativeConditionFactory : IConditionFactory;

[ComImport]
[Guid(ShellIIDGuid.IQueryParserManager)]
[CoClass(typeof(QueryParserManagerCoClass))]
public interface INativeQueryParserManager : IQueryParserManager;

[ComImport]
[Guid(ShellIIDGuid.ISearchFolderItemFactory)]
[CoClass(typeof(SearchFolderItemFactoryCoClass))]
public interface INativeSearchFolderItemFactory : ISearchFolderItemFactory;

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("00000109-0000-0000-C000-000000000046")]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
public interface IPersistStream
{
    [PreserveSig]
    public void GetClassID(out Guid pClassID);

    [PreserveSig]
    public HResult IsDirty();

    [PreserveSig]
    public HResult Load([In, MarshalAs(UnmanagedType.Interface)] IStream stm);

    [PreserveSig]
    public HResult Save([In, MarshalAs(UnmanagedType.Interface)] IStream stm, bool fRemember);

    [PreserveSig]
    public HResult GetSizeMax(out ulong cbSize);
}

[ComImport]
[Guid(ShellIIDGuid.IQueryParser)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
public interface IQueryParser
{
    [PreserveSig]
    public HResult Parse([In, MarshalAs(UnmanagedType.LPWStr)] string pszInputString, [In] IEnumUnknown pCustomProperties, [Out] out IQuerySolution ppSolution);

    [PreserveSig]
    public HResult SetOption([In] StructuredQuerySingleOption option, [In] PropVariant pOptionValue);

    [PreserveSig]
    public HResult GetOption([In] StructuredQuerySingleOption option, [Out] PropVariant pOptionValue);

    [PreserveSig]
    public HResult SetMultiOption([In] StructuredQueryMultipleOption option, [In, MarshalAs(UnmanagedType.LPWStr)] string pszOptionKey, [In] PropVariant pOptionValue);

    [PreserveSig]
    public HResult GetSchemaProvider([Out] out nint ppSchemaProvider);

    [PreserveSig]
    public HResult RestateToString([In] ICondition pCondition, [In] bool fUseEnglish, [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszQueryString);

    [PreserveSig]
    public HResult ParsePropertyValue([In, MarshalAs(UnmanagedType.LPWStr)] string pszPropertyName, [In, MarshalAs(UnmanagedType.LPWStr)] string pszInputString, [Out] out IQuerySolution ppSolution);

    [PreserveSig]
    public HResult RestatePropertyValueToString([In] ICondition pCondition, [In] bool fUseEnglish, [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszPropertyName, [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszQueryString);
}

[ComImport]
[Guid(ShellIIDGuid.IQueryParserManager)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
public interface IQueryParserManager
{
    [PreserveSig]
    public HResult CreateLoadedParser([In, MarshalAs(UnmanagedType.LPWStr)] string pszCatalog, [In] ushort langidForKeywords, [In] ref Guid riid, [Out] out IQueryParser ppQueryParser);

    [PreserveSig]
    public HResult InitializeOptions([In] bool fUnderstandNQS, [In] bool fAutoWildCard, [In] IQueryParser pQueryParser);

    [PreserveSig]
    public HResult SetOption([In] QueryParserManagerOption option, [In] PropVariant pOptionValue);
};

[ComImport]
[Guid(ShellIIDGuid.IQuerySolution)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IQuerySolution : IConditionFactory
{
    [PreserveSig]
    public HResult MakeNot([In] ICondition pcSub, [In] bool fSimplify, [Out] out ICondition ppcResult);

    [PreserveSig]
    public HResult MakeAndOr([In] SearchConditionType ct, [In] IEnumUnknown peuSubs, [In] bool fSimplify, [Out] out ICondition ppcResult);

    [PreserveSig]
    public HResult MakeLeaf(
        [In, MarshalAs(UnmanagedType.LPWStr)] string pszPropertyName,
        [In] SearchConditionOperation cop,
        [In, MarshalAs(UnmanagedType.LPWStr)] string pszValueType,
        [In] PropVariant ppropvar,
        IRichChunk richChunk1,
        IRichChunk richChunk2,
        IRichChunk richChunk3,
        [In] bool fExpand,
        [Out] out ICondition ppcResult);

    [PreserveSig]
    public HResult Resolve();

    [PreserveSig]
    public HResult GetQuery([Out, MarshalAs(UnmanagedType.Interface)] out ICondition ppQueryNode, [Out, MarshalAs(UnmanagedType.Interface)] out IEntity ppMainType);

    [PreserveSig]
    public HResult GetErrors([In] ref Guid riid, [Out] out nint ppParseErrors);

    [PreserveSig]
    public HResult GetLexicalData([MarshalAs(UnmanagedType.LPWStr)] out string ppszInputString, [Out] out nint ppTokens, [Out] out uint plcid, [Out] /* IUnknown** */ out nint ppWordBreaker);
}

[ComImport]
[Guid(ShellIIDGuid.IRichChunk)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
public interface IRichChunk
{
    [PreserveSig]
    public HResult GetData();
}

[ComImport]
[Guid(ShellIIDGuid.ISearchFolderItemFactory)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface ISearchFolderItemFactory
{
    [PreserveSig]
    public HResult SetScope([In, MarshalAs(UnmanagedType.Interface)] IShellItemArray ppv);

    [PreserveSig]
    public HResult SetCondition([In] ICondition pCondition);

    [PreserveSig]
    public int GetShellItem(ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out IShellItem ppv);
};

[ComImport]
[Guid(ShellIIDGuid.ISharedBitmap)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
public interface ISharedBitmap
{
    public void GetSharedBitmap([Out] out nint phbm);

    public void GetSize([Out] out SIZE pSize);

    public void GetFormat([Out] out ThumbnailAlphaType pat);

    public void InitializeBitmap([In] nint hbm, [In] ThumbnailAlphaType wtsAT);

    public void Detach([Out] out nint phbm);
}

[ComImport]
[Guid(ShellIIDGuid.IShellFolder)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComConversionLoss]
public interface IShellFolder
{
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void ParseDisplayName(nint hwnd, [In, MarshalAs(UnmanagedType.Interface)] IBindCtx pbc, [In, MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, [In, Out] ref uint pchEaten, [Out] nint ppidl, [In, Out] ref uint pdwAttributes);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult EnumObjects([In] nint hwnd, [In] ShellFolderEnumerationOptions grfFlags, [MarshalAs(UnmanagedType.Interface)] out IEnumIDList ppenumIDList);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult BindToObject([In] nint pidl, nint pbc, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out IShellFolder ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void BindToStorage([In] ref nint pidl, [In, MarshalAs(UnmanagedType.Interface)] IBindCtx pbc, [In] ref Guid riid, out nint ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void CompareIDs([In] nint lParam, [In] ref nint pidl1, [In] ref nint pidl2);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void CreateViewObject([In] nint hwndOwner, [In] ref Guid riid, out nint ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetAttributesOf([In] uint cidl, [In] nint apidl, [In, Out] ref uint rgfInOut);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetUIObjectOf([In] nint hwndOwner, [In] uint cidl, [In] nint apidl, [In] ref Guid riid, [In, Out] ref uint rgfReserved, out nint ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetDisplayNameOf([In] ref nint pidl, [In] uint uFlags, out nint pName);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SetNameOf([In] nint hwnd, [In] ref nint pidl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszName, [In] uint uFlags, [Out] nint ppidlOut);
}

[ComImport]
[Guid(ShellIIDGuid.IShellFolder2)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComConversionLoss]
public interface IShellFolder2 : IShellFolder
{
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void ParseDisplayName([In] nint hwnd, [In, MarshalAs(UnmanagedType.Interface)] IBindCtx pbc, [In, MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, [In, Out] ref uint pchEaten, [Out] nint ppidl, [In, Out] ref uint pdwAttributes);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void EnumObjects([In] nint hwnd, [In] ShellFolderEnumerationOptions grfFlags, [MarshalAs(UnmanagedType.Interface)] out IEnumIDList ppenumIDList);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void BindToObject([In] nint pidl, nint pbc, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out IShellFolder ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void BindToStorage([In] ref nint pidl, [In, MarshalAs(UnmanagedType.Interface)] IBindCtx pbc, [In] ref Guid riid, out nint ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void CompareIDs([In] nint lParam, [In] ref nint pidl1, [In] ref nint pidl2);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void CreateViewObject([In] nint hwndOwner, [In] ref Guid riid, out nint ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetAttributesOf([In] uint cidl, [In] nint apidl, [In, Out] ref uint rgfInOut);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetUIObjectOf([In] nint hwndOwner, [In] uint cidl, [In] nint apidl, [In] ref Guid riid, [In, Out] ref uint rgfReserved, out nint ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetDisplayNameOf([In] ref nint pidl, [In] uint uFlags, out nint pName);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SetNameOf([In] nint hwnd, [In] ref nint pidl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszName, [In] uint uFlags, [Out] nint ppidlOut);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetDefaultSearchGUID(out Guid pguid);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void EnumSearches([Out] out nint ppenum);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetDefaultColumn([In] uint dwRes, out uint pSort, out uint pDisplay);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetDefaultColumnState([In] uint iColumn, out uint pcsFlags);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetDetailsEx([In] ref nint pidl, [In] ref PropertyKey pscid, [MarshalAs(UnmanagedType.Struct)] out object pv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetDetailsOf([In] ref nint pidl, [In] uint iColumn, out nint psd);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void MapColumnToSCID([In] uint iColumn, out PropertyKey pscid);
}

[ComImport]
[Guid(ShellIIDGuid.IShellItem)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IShellItem
{
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult BindToHandler(
        [In] nint pbc,
        [In] ref Guid bhid,
        [In] ref Guid riid,
        [Out, MarshalAs(UnmanagedType.Interface)] out IShellFolder ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetParent([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetDisplayName(
        [In] ShellItemDesignNameOptions sigdnName,
        out nint ppszName);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetAttributes([In] ShellFileGetAttributesOptions sfgaoMask, out ShellFileGetAttributesOptions psfgaoAttribs);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult Compare(
        [In, MarshalAs(UnmanagedType.Interface)] IShellItem psi,
        [In] SICHINTF hint,
        out int piOrder);
}

[ComImport,
Guid(ShellIIDGuid.IShellItem2),
InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IShellItem2 : IShellItem
{
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult BindToHandler(
        [In] nint pbc,
        [In] ref Guid bhid,
        [In] ref Guid riid,
        [Out, MarshalAs(UnmanagedType.Interface)] out IShellFolder ppv);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetParent([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetDisplayName(
        [In] ShellItemDesignNameOptions sigdnName,
        [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetAttributes([In] ShellFileGetAttributesOptions sfgaoMask, out ShellFileGetAttributesOptions psfgaoAttribs);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void Compare(
        [In, MarshalAs(UnmanagedType.Interface)] IShellItem psi,
        [In] uint hint,
        out int piOrder);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), PreserveSig]
    public int GetPropertyStore(
        [In] GetPropertyStoreOptions Flags,
        [In] ref Guid riid,
        [Out, MarshalAs(UnmanagedType.Interface)] out IPropertyStore ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetPropertyStoreWithCreateObject([In] GetPropertyStoreOptions Flags, [In, MarshalAs(UnmanagedType.IUnknown)] object punkCreateObject, [In] ref Guid riid, out nint ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetPropertyStoreForKeys([In] ref PropertyKey rgKeys, [In] uint cKeys, [In] GetPropertyStoreOptions Flags, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.IUnknown)] out IPropertyStore ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetPropertyDescriptionList([In] ref PropertyKey keyType, [In] ref Guid riid, out nint ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult Update([In, MarshalAs(UnmanagedType.Interface)] IBindCtx pbc);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetProperty([In] ref PropertyKey key, [Out] PropVariant ppropvar);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetCLSID([In] ref PropertyKey key, out Guid pclsid);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetFileTime([In] ref PropertyKey key, out System.Runtime.InteropServices.ComTypes.FILETIME pft);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetInt32([In] ref PropertyKey key, out int pi);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetString([In] ref PropertyKey key, [MarshalAs(UnmanagedType.LPWStr)] out string ppsz);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetUInt32([In] ref PropertyKey key, out uint pui);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetUInt64([In] ref PropertyKey key, out ulong pull);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetBool([In] ref PropertyKey key, out int pf);
}

[ComImport]
[Guid(ShellIIDGuid.IShellItemArray)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IShellItemArray
{
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult BindToHandler(
        [In, MarshalAs(UnmanagedType.Interface)] nint pbc,
        [In] ref Guid rbhid,
        [In] ref Guid riid,
        out nint ppvOut);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetPropertyStore(
        [In] int Flags,
        [In] ref Guid riid,
        out nint ppv);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetPropertyDescriptionList(
        [In] ref PropertyKey keyType,
        [In] ref Guid riid,
        out nint ppv);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetAttributes(
        [In] ShellItemAttributeOptions dwAttribFlags,
        [In] ShellFileGetAttributesOptions sfgaoMask,
        out ShellFileGetAttributesOptions psfgaoAttribs);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetCount(out uint pdwNumItems);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetItemAt(
        [In] uint dwIndex,
        [MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult EnumItems([MarshalAs(UnmanagedType.Interface)] out nint ppenumShellItems);
}

[ComImport()]
[Guid("bcc18b79-ba16-442f-80c4-8a59c30c463b")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
public interface IShellItemImageFactory
{
    [PreserveSig]
    public HResult GetImage(
        [In, MarshalAs(UnmanagedType.Struct)] SIZE size,
        [In] SIIGBF flags,
        [Out] out nint phbm);
}

[ComImport]
[Guid(ShellIIDGuid.IShellLibrary)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IShellLibrary
{
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult LoadLibraryFromItem(
        [In, MarshalAs(UnmanagedType.Interface)] IShellItem library,
        [In] AccessModes grfMode);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void LoadLibraryFromKnownFolder(
        [In] ref Guid knownfidLibrary,
        [In] AccessModes grfMode);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void AddFolder([In, MarshalAs(UnmanagedType.Interface)] IShellItem location);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void RemoveFolder([In, MarshalAs(UnmanagedType.Interface)] IShellItem location);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetFolders(
        [In] LibraryFolderFilter lff,
        [In] ref Guid riid,
        [MarshalAs(UnmanagedType.Interface)] out IShellItemArray ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void ResolveFolder(
        [In, MarshalAs(UnmanagedType.Interface)] IShellItem folderToResolve,
        [In] uint timeout,
        [In] ref Guid riid,
        [MarshalAs(UnmanagedType.Interface)] out IShellItem ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetDefaultSaveFolder(
        [In] DefaultSaveFolderType dsft,
        [In] ref Guid riid,
        [MarshalAs(UnmanagedType.Interface)] out IShellItem ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SetDefaultSaveFolder(
        [In] DefaultSaveFolderType dsft,
        [In, MarshalAs(UnmanagedType.Interface)] IShellItem si);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetOptions(
        out LibraryOptions lofOptions);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SetOptions(
        [In] LibraryOptions lofMask,
        [In] LibraryOptions lofOptions);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetFolderType(out Guid ftid);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SetFolderType([In] ref Guid ftid);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetIcon([MarshalAs(UnmanagedType.LPWStr)] out string icon);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SetIcon([In, MarshalAs(UnmanagedType.LPWStr)] string icon);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void Commit();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void Save(
        [In, MarshalAs(UnmanagedType.Interface)] IShellItem folderToSaveIn,
        [In, MarshalAs(UnmanagedType.LPWStr)] string libraryName,
        [In] LibrarySaveOptions lsf,
        [MarshalAs(UnmanagedType.Interface)] out IShellItem2 savedTo);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SaveInKnownFolder(
        [In] ref Guid kfidToSaveIn,
        [In, MarshalAs(UnmanagedType.LPWStr)] string libraryName,
        [In] LibrarySaveOptions lsf,
        [MarshalAs(UnmanagedType.Interface)] out IShellItem2 savedTo);
};

[ComImport]
[Guid(ShellIIDGuid.IShellLinkW)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IShellLinkW
{
    public void GetPath(
        [Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile,
        int cchMaxPath,
        nint pfd,
        uint fFlags);

    public void GetIDList(out nint ppidl);

    public void SetIDList(nint pidl);

    public void GetDescription(
        [Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile,
        int cchMaxName);

    public void SetDescription(
        [MarshalAs(UnmanagedType.LPWStr)] string pszName);

    public void GetWorkingDirectory(
        [Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir,
        int cchMaxPath
        );

    public void SetWorkingDirectory(
        [MarshalAs(UnmanagedType.LPWStr)] string pszDir);

    public void GetArguments(
        [Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs,
        int cchMaxPath);

    public void SetArguments(
        [MarshalAs(UnmanagedType.LPWStr)] string pszArgs);

    public void GetHotKey(out short wHotKey);

    public void SetHotKey(short wHotKey);

    public void GetShowCmd(out uint iShowCmd);

    public void SetShowCmd(uint iShowCmd);

    public void GetIconLocation(
        [Out(), MarshalAs(UnmanagedType.LPWStr)] out StringBuilder pszIconPath,
        int cchIconPath,
        out int iIcon);

    public void SetIconLocation(
        [MarshalAs(UnmanagedType.LPWStr)] string pszIconPath,
        int iIcon);

    public void SetRelativePath(
        [MarshalAs(UnmanagedType.LPWStr)] string pszPathRel,
        uint dwReserved);

    public void Resolve(nint hwnd, uint fFlags);

    public void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
}

[ComImport]
[Guid(ShellIIDGuid.IThumbnailCache)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
public interface IThumbnailCache
{
    public void GetThumbnail([In] IShellItem pShellItem,
        [In] uint cxyRequestedThumbSize,
        [In] ThumbnailOptions flags,
        [Out] out ISharedBitmap ppvThumb,
        [Out] out ThumbnailCacheOptions pOutFlags,
        [Out] ThumbnailId pThumbnailID);

    public void GetThumbnailByID([In] ThumbnailId thumbnailID,
        [In] uint cxyRequestedThumbSize,
        [Out] out ISharedBitmap ppvThumb,
        [Out] out ThumbnailCacheOptions pOutFlags);
}

[ComImport]
[ClassInterface(ClassInterfaceType.None)]
[TypeLibType(TypeLibTypeFlags.FCanCreate)]
[Guid(ShellCLSIDGuid.ConditionFactory)]
public class ConditionFactoryCoClass;

[ComImport]
[Guid(ShellIIDGuid.CShellLink)]
[ClassInterface(ClassInterfaceType.None)]
public class CShellLink;

[ComImport]
[ClassInterface(ClassInterfaceType.None)]
[TypeLibType(TypeLibTypeFlags.FCanCreate)]
[Guid(ShellCLSIDGuid.QueryParserManager)]
public class QueryParserManagerCoClass;

[ComImport]
[ClassInterface(ClassInterfaceType.None)]
[TypeLibType(TypeLibTypeFlags.FCanCreate)]
[Guid(ShellCLSIDGuid.SearchFolderItemFactory)]
public class SearchFolderItemFactoryCoClass;
