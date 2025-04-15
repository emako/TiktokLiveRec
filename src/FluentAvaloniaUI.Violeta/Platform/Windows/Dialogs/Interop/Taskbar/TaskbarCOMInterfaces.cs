using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Taskbar;

[ComImport]
[Guid("6332DEBF-87B5-4670-90C0-5E57B408A49E")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface ICustomDestinationList
{
    public void SetAppID(
        [MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

    [PreserveSig]
    public HResult BeginList(
        out uint cMaxSlots,
        ref Guid riid,
        [Out(), MarshalAs(UnmanagedType.Interface)] out object ppvObject);

    [PreserveSig]
    public HResult AppendCategory(
        [MarshalAs(UnmanagedType.LPWStr)] string pszCategory,
        [MarshalAs(UnmanagedType.Interface)] IObjectArray poa);

    public void AppendKnownCategory(
        [MarshalAs(UnmanagedType.I4)] KnownDestinationCategory category);

    [PreserveSig]
    public HResult AddUserTasks(
        [MarshalAs(UnmanagedType.Interface)] IObjectArray poa);

    public void CommitList();

    public void GetRemovedDestinations(
        ref Guid riid,
        [Out(), MarshalAs(UnmanagedType.Interface)] out object ppvObject);

    public void DeleteList(
        [MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

    public void AbortList();
}

[ComImport]
[Guid("92CA9DCD-5622-4BBA-A805-5E9F541BD8C9")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
internal interface IObjectArray
{
    public void GetCount(out uint cObjects);

    public void GetAt(
        uint iIndex,
        ref Guid riid,
        [Out(), MarshalAs(UnmanagedType.Interface)] out object ppvObject);
}

[ComImport]
[Guid("5632B1A4-E38A-400A-928A-D4CD63230295")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IObjectCollection
{
    [PreserveSig]
    public void GetCount(out uint cObjects);

    [PreserveSig]
    public void GetAt(
        uint iIndex,
        ref Guid riid,
        [Out(), MarshalAs(UnmanagedType.Interface)] out object ppvObject);

    public void AddObject(
        [MarshalAs(UnmanagedType.Interface)] object pvObject);

    public void AddFromArray(
        [MarshalAs(UnmanagedType.Interface)] IObjectArray poaSource);

    public void RemoveObject(uint uiIndex);

    public void Clear();
}

[ComImport]
[Guid("c43dc798-95d1-4bea-9030-bb99e2983a1a")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
internal interface ITaskbarList4
{
    [PreserveSig]
    public void HrInit();

    [PreserveSig]
    public void AddTab(nint hwnd);

    [PreserveSig]
    public void DeleteTab(nint hwnd);

    [PreserveSig]
    public void ActivateTab(nint hwnd);

    [PreserveSig]
    public void SetActiveAlt(nint hwnd);

    [PreserveSig]
    public void MarkFullscreenWindow(
        nint hwnd,
        [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

    [PreserveSig]
    public void SetProgressValue(nint hwnd, ulong ullCompleted, ulong ullTotal);

    [PreserveSig]
    public void SetProgressState(nint hwnd, TaskbarProgressBarStatus tbpFlags);

    [PreserveSig]
    public void RegisterTab(nint hwndTab, nint hwndMDI);

    [PreserveSig]
    public void UnregisterTab(nint hwndTab);

    [PreserveSig]
    public void SetTabOrder(nint hwndTab, nint hwndInsertBefore);

    [PreserveSig]
    public void SetTabActive(nint hwndTab, nint hwndInsertBefore, uint dwReserved);

    [PreserveSig]
    public HResult ThumbBarAddButtons(
        nint hwnd,
        uint cButtons,
        [MarshalAs(UnmanagedType.LPArray)] ThumbButton[] pButtons);

    [PreserveSig]
    public HResult ThumbBarUpdateButtons(
        nint hwnd,
        uint cButtons,
        [MarshalAs(UnmanagedType.LPArray)] ThumbButton[] pButtons);

    [PreserveSig]
    public void ThumbBarSetImageList(nint hwnd, nint himl);

    [PreserveSig]
    public void SetOverlayIcon(
      nint hwnd,
      nint hIcon,
      [MarshalAs(UnmanagedType.LPWStr)] string pszDescription);

    [PreserveSig]
    public void SetThumbnailTooltip(
        nint hwnd,
        [MarshalAs(UnmanagedType.LPWStr)] string pszTip);

    [PreserveSig]
    public void SetThumbnailClip(
        nint hwnd,
        nint prcClip);

    public void SetTabProperties(nint hwndTab, SetTabPropertiesOption stpFlags);
}

[Guid("77F10CF0-3DB5-4966-B520-B7C54FD35ED6")]
[ClassInterface(ClassInterfaceType.None)]
[ComImport]
internal class CDestinationList;

[Guid("2D3468C1-36A7-43B6-AC24-D3F02FD9607A")]
[ClassInterface(ClassInterfaceType.None)]
[ComImport]
internal class CEnumerableObjectCollection;

[Guid("56FDF344-FD6D-11d0-958A-006097C9A090")]
[ClassInterface(ClassInterfaceType.None)]
[ComImport]
internal class CTaskbarList;
