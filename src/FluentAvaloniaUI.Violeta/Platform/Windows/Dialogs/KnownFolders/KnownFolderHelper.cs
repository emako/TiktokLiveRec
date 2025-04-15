using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;
using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.KnownFolders;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.KnownFolders;

public static class KnownFolderHelper
{
    [SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance")]
    public static IKnownFolder FromCanonicalName(string canonicalName)
    {
        var knownFolderManager = (IKnownFolderManager)new KnownFolderManagerClass();

        knownFolderManager.GetFolderByName(canonicalName, out var knownFolderNative);
        var kf = KnownFolderHelper.GetKnownFolder(knownFolderNative);
        return kf ?? throw new ArgumentException(LocalizedMessages.ShellInvalidCanonicalName, nameof(canonicalName));
    }

    public static IKnownFolder FromKnownFolderId(Guid knownFolderId)
    {
        var knownFolderManager = new KnownFolderManagerClass();

        var hr = knownFolderManager.GetFolder(knownFolderId, out var knownFolderNative);
        if (hr != HResult.Ok) { throw new ShellException(hr); }

        var kf = GetKnownFolder(knownFolderNative);
        return kf ?? throw new ArgumentException(LocalizedMessages.KnownFolderInvalidGuid, nameof(knownFolderId));
    }

    public static IKnownFolder FromParsingName(string parsingName)
    {
        ArgumentNullException.ThrowIfNull(parsingName);

        nint pidl = 0;
        nint pidl2 = 0;

        try
        {
            pidl = ShellHelper.PidlFromParsingName(parsingName);

            if (pidl == 0)
            {
                throw new ArgumentException(LocalizedMessages.KnownFolderParsingName, nameof(parsingName));
            }

            var knownFolderNative = FromPIDL(pidl);
            if (knownFolderNative != null)
            {
                var kf = GetKnownFolder(knownFolderNative);
                return kf ?? throw new ArgumentException(LocalizedMessages.KnownFolderParsingName, "parsingName");
            }

            pidl2 = ShellHelper.PidlFromParsingName(parsingName.PadRight(1, '\0'));

            if (pidl2 == 0)
            {
                throw new ArgumentException(LocalizedMessages.KnownFolderParsingName, nameof(parsingName));
            }

            var kf2 = GetKnownFolder(FromPIDL(pidl));
            return kf2 ?? throw new ArgumentException(LocalizedMessages.KnownFolderParsingName, "parsingName");
        }
        finally
        {
            Shell32.ILFree(pidl);
            Shell32.ILFree(pidl2);
        }
    }

    public static IKnownFolder FromPath(string path) => KnownFolderHelper.FromParsingName(path);

    [SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance")]
    internal static IKnownFolder FromKnownFolderIdInternal(Guid knownFolderId)
    {
        var knownFolderManager = (IKnownFolderManager)new KnownFolderManagerClass();

        var hr = knownFolderManager.GetFolder(knownFolderId, out var knownFolderNative);

        return (hr == HResult.Ok) ? GetKnownFolder(knownFolderNative) : null!;
    }

    internal static IKnownFolderNative FromPIDL(nint pidl)
    {
        var knownFolderManager = new KnownFolderManagerClass();

        var hr = knownFolderManager.FindFolderFromIDList(pidl, out var knownFolder);

        return (hr == HResult.Ok) ? knownFolder : null!;
    }

    private static IKnownFolder GetKnownFolder(IKnownFolderNative knownFolderNative)
    {
        Debug.Assert(knownFolderNative != null, "Native IKnownFolder should not be null.");

        var guid = new Guid(ShellIIDGuid.IShellItem2);
        HResult hr = knownFolderNative!.GetShellItem(0, ref guid, out var shellItem);

        if (!CoreErrorHelper.Succeeded(hr)) { return null!; }

        var isFileSystem = false;

        if (shellItem != null)
        {
            shellItem.GetAttributes(ShellFileGetAttributesOptions.FileSystem, out var sfgao);

            isFileSystem = (sfgao & ShellFileGetAttributesOptions.FileSystem) != 0;
        }

        if (isFileSystem)
        {
            var kf = new FileSystemKnownFolder(knownFolderNative);
            return kf;
        }

        var knownFsFolder = new NonFileSystemKnownFolder(knownFolderNative);
        return knownFsFolder;
    }
}
