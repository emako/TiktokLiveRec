using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;

public class ShellFileSystemFolder : ShellFolder
{
    internal ShellFileSystemFolder()
    {
    }

    internal ShellFileSystemFolder(IShellItem2 shellItem) => nativeShellItem = shellItem;

    public virtual string Path => ParsingName;

    public static ShellFileSystemFolder FromFolderPath(string path)
    {
        var absPath = ShellHelper.GetAbsolutePath(path);

        if (!Directory.Exists(absPath))
        {
            throw new DirectoryNotFoundException(
                string.Format(System.Globalization.CultureInfo.InvariantCulture,
                LocalizedMessages.FilePathNotExist, path));
        }

        var folder = new ShellFileSystemFolder();
        try
        {
            folder.ParsingName = absPath;
            return folder;
        }
        catch
        {
            folder.Dispose();
            throw;
        }
    }
}
