using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace TiktokLiveRec.Extensions;

public static class ShortcutHelper
{
    public static void CreateShortcut(string directory, string shortcutName, string targetPath, string arguments = null!, string description = null!, string iconLocation = null!)
    {
        if (!Directory.Exists(directory))
        {
            _ = Directory.CreateDirectory(directory);
        }

        string shortcutPath = Path.Combine(directory, $"{shortcutName}.lnk");

        dynamic? shell = null;
        dynamic? shortcut = null;

        try
        {
            shell = Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8"))!);
            shortcut = shell?.CreateShortcut(shortcutPath)!;
            shortcut.TargetPath = targetPath;
            shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);
            shortcut.WindowStyle = 1;
            shortcut.Arguments = arguments;
            shortcut.Description = description;
            shortcut.IconLocation = string.IsNullOrWhiteSpace(iconLocation) ? targetPath : iconLocation;
            shortcut.Save();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
        finally
        {
            if (shortcut != null)
            {
                _ = Marshal.FinalReleaseComObject(shortcut);
            }
            if (shell != null)
            {
                _ = Marshal.FinalReleaseComObject(shell);
            }
        }
    }

    public static void CreateShortcutOnDesktop(string shortcutName, string targetPath, string arguments = null!, string description = null!, string iconLocation = null!)
    {
        string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        CreateShortcut(desktop, shortcutName, targetPath, arguments, description, iconLocation);
    }
}
