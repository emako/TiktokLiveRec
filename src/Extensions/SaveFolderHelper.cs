using System.Diagnostics;
using System.IO;

namespace TiktokLiveRec.Extensions;

internal static class SaveFolderHelper
{
    public static string GetSaveFolder(string? settingsFolder = null)
    {
        if (string.IsNullOrWhiteSpace(settingsFolder))
        {
            const string path = "downloads";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return Path.GetFullPath(path);
        }
        else
        {
            try
            {
                if (!Directory.Exists(settingsFolder))
                {
                    Directory.CreateDirectory(settingsFolder);
                }

                return Path.GetFullPath(settingsFolder);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return GetSaveFolder(null);
            }
        }
    }
}
