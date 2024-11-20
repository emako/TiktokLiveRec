using System.IO;
using System.Text.RegularExpressions;

namespace TiktokLiveRec.Extensions;

internal static class SearchFileHelper
{
    public static IEnumerable<string> SearchFiles(string directory, string regexPattern, bool searchSubdirectories = true)
    {
        try
        {
            string[] files = Directory.GetFiles(directory, "*",
                searchSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            Regex regex = new(regexPattern, RegexOptions.IgnoreCase);
            return files.Where(file => regex.IsMatch(Path.GetFileName(file)));
        }
        catch (UnauthorizedAccessException e)
        {
            Console.WriteLine($"Unauthorized: {directory}, Detail: {e.Message}");
            return [];
        }
    }
}
