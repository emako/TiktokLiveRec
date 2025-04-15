using FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Interop.Common;
using System.Diagnostics.CodeAnalysis;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;

public class ShellFile : ShellObject
{
    [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    internal ShellFile(string path)
    {
        var absPath = ShellHelper.GetAbsolutePath(path);

        if (!File.Exists(absPath))
        {
            throw new FileNotFoundException(
                string.Format(System.Globalization.CultureInfo.InvariantCulture,
                LocalizedMessages.FilePathNotExist, path));
        }

        ParsingName = absPath;
    }

    internal ShellFile(IShellItem2 shellItem) => nativeShellItem = shellItem;

    public virtual string Path => ParsingName;

    public static ShellFile FromFilePath(string path) => new ShellFile(path);
}
