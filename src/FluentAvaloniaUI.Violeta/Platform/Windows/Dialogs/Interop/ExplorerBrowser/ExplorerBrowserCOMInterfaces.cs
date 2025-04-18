using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.ShellExtensions;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

#pragma warning disable CS0108

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.ExplorerBrowser;

[SupportedOSPlatform("Windows")]
internal enum CommDlgBrowser2ViewFlags
{
    ShowAllFiles = 0x00000001,
    IsFileSave = 0x00000002,
    AllowPreviewPane = 0x00000004,
    NoSelectVerb = 0x00000008,
    NoIncludeItem = 0x00000010,
    IsFolderPicker = 0x00000020,
}

[SupportedOSPlatform("Windows")]
internal enum CommDlgBrowserNotifyType
{
    Done = 1,
    Start = 2,
}

[SupportedOSPlatform("Windows")]
internal enum CommDlgBrowserStateChange
{
    SetFocus = 0,
    KillFocus = 1,
    SelectionChange = 2,
    Rename = 3,
    StateChange = 4,
}

[SupportedOSPlatform("Windows")]
[Flags]
internal enum ExplorerBrowserOptions
{
    NavigateOnce = 0x00000001,
    ShowFrames = 0x00000002,
    AlwaysNavigate = 0x00000004,
    NoTravelLog = 0x00000008,
    NoWrapperWindow = 0x00000010,
    HtmlSharepointView = 0x00000020,
    NoBorder = 0x00000040,
    NoPersistViewState = 0x00000080,
}

[SupportedOSPlatform("Windows")]
internal enum ExplorerPaneState
{
    DoNotCare = 0x00000000,
    DefaultOn = 0x00000001,
    DefaultOff = 0x00000002,
    StateMask = 0x0000ffff,
    InitialState = 0x00010000,
    Force = 0x00020000,
}

[SupportedOSPlatform("Windows")]
[Flags]
internal enum FolderOptions
{
    AutoArrange = 0x00000001,
    AbbreviatedNames = 0x00000002,
    SnapToGrid = 0x00000004,
    OwnerData = 0x00000008,
    BestFitWindow = 0x00000010,
    Desktop = 0x00000020,
    SingleSelection = 0x00000040,
    NoSubfolders = 0x00000080,
    Transparent = 0x00000100,
    NoClientEdge = 0x00000200,
    NoScroll = 0x00000400,
    AlignLeft = 0x00000800,
    NoIcons = 0x00001000,
    ShowSelectionAlways = 0x00002000,
    NoVisible = 0x00004000,
    SingleClickActivate = 0x00008000,
    NoWebView = 0x00010000,
    HideFilenames = 0x00020000,
    CheckSelect = 0x00040000,
    NoEnumRefresh = 0x00080000,
    NoGrouping = 0x00100000,
    FullRowSelect = 0x00200000,
    NoFilters = 0x00400000,
    NoColumnHeaders = 0x00800000,
    NoHeaderInAllViews = 0x01000000,
    ExtendedTiles = 0x02000000,
    TriCheckSelect = 0x04000000,
    AutoCheckSelect = 0x08000000,
    NoBrowserViewState = 0x10000000,
    SubsetGroups = 0x20000000,
    UseSearchFolders = 0x40000000,
    AllowRightToLeftReading = unchecked((int)0x80000000),
}

[SupportedOSPlatform("Windows")]
[SuppressMessage("Design", "CA1069:Enums values should not be duplicated")]
internal enum FolderViewMode
{
    Auto = -1,
    First = 1,
    Icon = 1,
    SmallIcon = 2,
    List = 3,
    Details = 4,
    Thumbnail = 5,
    Tile = 6,
    Thumbstrip = 7,
    Content = 8,
    Last = 8,
}

[SupportedOSPlatform("Windows")]
internal enum ShellViewGetItemObject
{
    Background = 0x00000000,
    Selection = 0x00000001,
    AllView = 0x00000002,
    Checked = 0x00000003,
    TypeMask = 0x0000000F,
    ViewOrderFlag = unchecked((int)0x80000000),
}

[SupportedOSPlatform("Windows")]
[ComImport]
[Guid(ExplorerBrowserIIDGuid.ICommDlgBrowser3)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface ICommDlgBrowser3
{
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult OnDefaultCommand(nint ppshv);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult OnStateChange(
        nint ppshv,
        CommDlgBrowserStateChange uChange);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult IncludeObject(
        nint ppshv,
        nint pidl);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetDefaultMenuText(
        IShellView shellView,
        nint buffer,
        int bufferMaxLength);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetViewFlags(
        [Out] out uint pdwFlags);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult Notify(
        nint pshv, CommDlgBrowserNotifyType notifyType);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetCurrentFilter(
        StringBuilder pszFileSpec,
        int cchFileSpec);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult OnColumnClicked(
        IShellView ppshv,
        int iColumn);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult OnPreViewCreated(IShellView ppshv);
}

[SupportedOSPlatform("Windows")]
[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid(ExplorerBrowserIIDGuid.IExplorerBrowser)]
internal interface IExplorerBrowser
{
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void Initialize(nint hwndParent, [In] ref RECT prc, [In] FolderSettings pfs);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void Destroy();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SetRect([In, Out] ref nint phdwp, RECT rcBrowser);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SetPropertyBag([MarshalAs(UnmanagedType.LPWStr)] string pszPropertyBag);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SetEmptyText([MarshalAs(UnmanagedType.LPWStr)] string pszEmptyText);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult SetFolderSettings(FolderSettings pfs);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult Advise(nint psbe, out uint pdwCookie);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult Unadvise([In] uint dwCookie);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SetOptions([In] ExplorerBrowserOptions dwFlag);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetOptions(out ExplorerBrowserOptions pdwFlag);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void BrowseToIDList(nint pidl, uint uFlags);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult BrowseToObject([MarshalAs(UnmanagedType.IUnknown)] object punk, uint uFlags);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void FillFromObject([MarshalAs(UnmanagedType.IUnknown)] object punk, int dwFlags);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void RemoveAll();

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetCurrentView(ref Guid riid, out nint ppv);
}

[SupportedOSPlatform("Windows")]
[ComImport]
[Guid(ExplorerBrowserIIDGuid.IExplorerBrowserEvents)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IExplorerBrowserEvents
{
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult OnNavigationPending(nint pidlFolder);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult OnViewCreated([MarshalAs(UnmanagedType.IUnknown)] object psv);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult OnNavigationComplete(nint pidlFolder);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult OnNavigationFailed(nint pidlFolder);
}

[SupportedOSPlatform("Windows")]
[ComImport]
[Guid(ExplorerBrowserIIDGuid.IExplorerPaneVisibility)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
internal interface IExplorerPaneVisibility
{
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetPaneState(ref Guid explorerPane, out ExplorerPaneState peps);
};

[SupportedOSPlatform("Windows")]
[ComImport]
[Guid(ExplorerBrowserIIDGuid.IFolderView)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IFolderView
{
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetCurrentViewMode([Out] out uint pViewMode);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SetCurrentViewMode(uint ViewMode);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetFolder(ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void Item(int iItemIndex, out nint ppidl);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void ItemCount(uint uFlags, out int pcItems);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void Items(uint uFlags, ref Guid riid, [Out, MarshalAs(UnmanagedType.IUnknown)] out object ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetSelectionMarkedItem(out int piItem);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetFocusedItem(out int piItem);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetItemPosition(nint pidl, out POINT ppt);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetSpacing([Out] out POINT ppt);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetDefaultSpacing(out POINT ppt);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetAutoArrange();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SelectItem(int iItem, uint dwFlags);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SelectAndPositionItems(uint cidl, nint apidl, ref POINT apt, uint dwFlags);
}

[SupportedOSPlatform("Windows")]
[ComImport]
[Guid(ExplorerBrowserIIDGuid.IFolderView2)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IFolderView2 : IFolderView
{
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetCurrentViewMode(out uint pViewMode);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SetCurrentViewMode(uint ViewMode);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetFolder(ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void Item(int iItemIndex, out nint ppidl);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult ItemCount(uint uFlags, out int pcItems);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult Items(uint uFlags, ref Guid riid, [Out, MarshalAs(UnmanagedType.IUnknown)] out object ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetSelectionMarkedItem(out int piItem);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetFocusedItem(out int piItem);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetItemPosition(nint pidl, out POINT ppt);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetSpacing([Out] out POINT ppt);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetDefaultSpacing(out POINT ppt);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetAutoArrange();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SelectItem(int iItem, uint dwFlags);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SelectAndPositionItems(uint cidl, nint apidl, ref POINT apt, uint dwFlags);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SetGroupBy(nint key, bool fAscending);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetGroupBy(ref nint pkey, ref bool pfAscending);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SetViewProperty(nint pidl, nint propkey, object propvar);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetViewProperty(nint pidl, nint propkey, out object ppropvar);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SetTileViewProperties(nint pidl, [MarshalAs(UnmanagedType.LPWStr)] string pszPropList);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SetExtendedTileViewProperties(nint pidl, [MarshalAs(UnmanagedType.LPWStr)] string pszPropList);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SetText(int iType, [MarshalAs(UnmanagedType.LPWStr)] string pwszText);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SetCurrentFolderFlags(uint dwMask, uint dwFlags);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetCurrentFolderFlags(out uint pdwFlags);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetSortColumnCount(out int pcColumns);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SetSortColumns(nint rgSortColumns, int cColumns);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetSortColumns(out nint rgSortColumns, int cColumns);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetItem(int iItem, ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetVisibleItem(int iStart, bool fPrevious, out int piItem);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetSelectedItem(int iStart, out int piItem);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetSelection(bool fNoneImpliesFolder, out IShellItemArray ppsia);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetSelectionState(nint pidl, out uint pdwFlags);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void InvokeVerbOnSelection([In, MarshalAs(UnmanagedType.LPWStr)] string pszVerb);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult SetViewModeAndIconSize(int uViewMode, int iImageSize);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetViewModeAndIconSize(out int puViewMode, out int piImageSize);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SetGroupSubsetCount(uint cVisibleRows);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void GetGroupSubsetCount(out uint pcVisibleRows);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void SetRedraw(bool fRedrawOn);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void IsMoveInSameFolder();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public void DoRename();
}

[SupportedOSPlatform("Windows")]
[ComImport]
[Guid(ExplorerBrowserIIDGuid.IInputObject)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
internal interface IInputObject
{
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult UIActivateIO(bool fActivate, ref MESSAGE pMsg);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult HasFocusIO();

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult TranslateAcceleratorIO(ref MESSAGE pMsg);
};

[SupportedOSPlatform("Windows")]
[ComImport]
[Guid(ExplorerBrowserIIDGuid.IServiceProvider)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[SuppressMessage("Interoperability", "SYSLIB1096:Convert to 'GeneratedComInterface'")]
internal interface IServiceProvider
{
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public HResult QueryService(ref Guid guidService, ref Guid riid, out nint ppvObject);
};

[SupportedOSPlatform("Windows")]
[ComImport]
[Guid(ExplorerBrowserIIDGuid.IShellView)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IShellView
{
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetWindow(
        out nint phwnd);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult ContextSensitiveHelp(
        bool fEnterMode);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult TranslateAccelerator(
        nint pmsg);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult EnableModeless(
        bool fEnable);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult UIActivate(
        uint uState);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult Refresh();

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult CreateViewWindow(
        [MarshalAs(UnmanagedType.IUnknown)] object psvPrevious,
        nint pfs,
        [MarshalAs(UnmanagedType.IUnknown)] object psb,
        nint prcView,
        out nint phWnd);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult DestroyViewWindow();

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetCurrentInfo(
        out nint pfs);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult AddPropertySheetPages(
        uint dwReserved,
        nint pfn,
        uint lparam);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult SaveViewState();

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult SelectItem(
        nint pidlItem,
        uint uFlags);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public HResult GetItemObject(
        ShellViewGetItemObject uItem,
        ref Guid riid,
        [MarshalAs(UnmanagedType.IUnknown)] out object ppv);
}

[SupportedOSPlatform("Windows")]
[ComImport]
[TypeLibType(TypeLibTypeFlags.FCanCreate)]
[ClassInterface(ClassInterfaceType.None)]
[Guid(ExplorerBrowserCLSIDGuid.ExplorerBrowser)]
internal class ExplorerBrowserClass : IExplorerBrowser
{
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void Initialize(nint hwndParent, [In] ref RECT prc, [In] FolderSettings pfs);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void Destroy();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void SetRect([In, Out] ref nint phdwp, RECT rcBrowser);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void SetPropertyBag([MarshalAs(UnmanagedType.LPWStr)] string pszPropertyBag);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void SetEmptyText([MarshalAs(UnmanagedType.LPWStr)] string pszEmptyText);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual HResult SetFolderSettings(FolderSettings pfs);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual HResult Advise(nint psbe, out uint pdwCookie);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual HResult Unadvise(uint dwCookie);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void SetOptions([In] ExplorerBrowserOptions dwFlag);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void GetOptions(out ExplorerBrowserOptions pdwFlag);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void BrowseToIDList(nint pidl, uint uFlags);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual HResult BrowseToObject([MarshalAs(UnmanagedType.IUnknown)] object punk, uint uFlags);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void FillFromObject([MarshalAs(UnmanagedType.IUnknown)] object punk, int dwFlags);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual void RemoveAll();

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern virtual HResult GetCurrentView(ref Guid riid, out nint ppv);
}

[SupportedOSPlatform("Windows")]
[StructLayout(LayoutKind.Sequential, Pack = 4)]
internal class FolderSettings
{
    public FolderViewMode ViewMode;
    public FolderOptions Options;
}
