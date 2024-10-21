using System.IO;

namespace TiktokLiveRec.Extensions;

internal class SearchFileHelper
{
    public static IEnumerable<string> SearchFiles(string directory, string fileName)
    {
        List<string> foundFiles = [];

        try
        {
            foreach (var file in Directory.GetFiles(directory, fileName))
            {
                foundFiles.Add(Path.GetFullPath(file));
            }

            foreach (var subDirectory in Directory.GetDirectories(directory))
            {
                foundFiles.AddRange(SearchFiles(subDirectory, fileName));
            }
        }
        catch (UnauthorizedAccessException e)
        {
            Console.WriteLine($"访问被拒绝: {directory}, 错误: {e.Message}");
        }

        return foundFiles;
    }
}
