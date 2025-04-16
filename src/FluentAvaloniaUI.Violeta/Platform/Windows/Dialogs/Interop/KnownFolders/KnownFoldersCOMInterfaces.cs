using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.KnownFolders;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

#pragma warning disable CS0108

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.KnownFolders;

[SupportedOSPlatform("Windows")]
[ComImport]
[Guid(KnownFoldersIIDGuid.IKnownFolderManager)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IKnownFolderManager
{
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void FolderIdFromCsidl(int csidl,
       [Out] out Guid knownFolderID);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void FolderIdToCsidl([In, MarshalAs(UnmanagedType.LPStruct)] Guid id,
      [Out] out int csidl);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetFolderIds([Out] out nint folders,
      [Out] out uint count);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetFolder([In, MarshalAs(UnmanagedType.LPStruct)] Guid id,
      [Out, MarshalAs(UnmanagedType.Interface)] out IKnownFolderNative knownFolder);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetFolderByName(string canonicalName,
      [Out, MarshalAs(UnmanagedType.Interface)] out IKnownFolderNative knownFolder);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void RegisterFolder(
        [In, MarshalAs(UnmanagedType.LPStruct)] Guid knownFolderGuid,
        [In] ref KnownFoldersSafeNativeMethods.NativeFolderDefinition knownFolderDefinition);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void UnregisterFolder(
        [In, MarshalAs(UnmanagedType.LPStruct)] Guid knownFolderGuid);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void FindFolderFromPath(
        [In, MarshalAs(UnmanagedType.LPWStr)] string path,
        [In] int mode,
        [Out, MarshalAs(UnmanagedType.Interface)] out IKnownFolderNative knownFolder);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult FindFolderFromIDList(nint pidl, [Out, MarshalAs(UnmanagedType.Interface)] out IKnownFolderNative knownFolder);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void Redirect();
}

[SupportedOSPlatform("Windows")]
[ComImport]
[Guid(KnownFoldersIIDGuid.IKnownFolder)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IKnownFolderNative
{
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public Guid GetId();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public FolderCategory GetCategory();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [PreserveSig]
    public HResult GetShellItem([In] int i,
         ref Guid interfaceGuid,
         [Out, MarshalAs(UnmanagedType.Interface)] out IShellItem2 shellItem);

    [return: MarshalAs(UnmanagedType.LPWStr)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public string GetPath([In] int option);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SetPath([In] int i, [In] string path);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetIDList([In] int i,
        [Out] out nint itemIdentifierListPointer);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public Guid GetFolderType();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public RedirectionCapability GetRedirectionCapabilities();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetFolderDefinition(
        [Out, MarshalAs(UnmanagedType.Struct)] out KnownFoldersSafeNativeMethods.NativeFolderDefinition definition);
}

[SupportedOSPlatform("Windows")]
[ComImport]
[Guid("4df0c730-df9d-4ae3-9153-aa6b82e9795a")]
internal class KnownFolderManagerClass : IKnownFolderManager
{
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void FolderIdFromCsidl(int csidl,
        [Out] out Guid knownFolderID);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void FolderIdToCsidl(
        [In, MarshalAs(UnmanagedType.LPStruct)] Guid id,
        [Out] out int csidl);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void GetFolderIds(
        [Out] out nint folders,
        [Out] out uint count);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual HResult GetFolder(
        [In, MarshalAs(UnmanagedType.LPStruct)] Guid id,
        [Out, MarshalAs(UnmanagedType.Interface)]
          out IKnownFolderNative knownFolder);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void GetFolderByName(
        string canonicalName,
        [Out, MarshalAs(UnmanagedType.Interface)] out IKnownFolderNative knownFolder);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void RegisterFolder(
        [In, MarshalAs(UnmanagedType.LPStruct)] Guid knownFolderGuid,
        [In] ref KnownFoldersSafeNativeMethods.NativeFolderDefinition knownFolderDefinition);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void UnregisterFolder(
        [In, MarshalAs(UnmanagedType.LPStruct)] Guid knownFolderGuid);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void FindFolderFromPath(
        [In, MarshalAs(UnmanagedType.LPWStr)] string path,
        [In] int mode,
        [Out, MarshalAs(UnmanagedType.Interface)] out IKnownFolderNative knownFolder);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual HResult FindFolderFromIDList(nint pidl, [Out, MarshalAs(UnmanagedType.Interface)] out IKnownFolderNative knownFolder);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void Redirect();
}
