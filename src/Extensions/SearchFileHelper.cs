using System.IO;

namespace TiktokLiveRec.Extensions;

internal static class SearchFileHelper
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
            Console.WriteLine($"Unauthorized: {directory}, Detail: {e.Message}");
        }

        return foundFiles;
    }
}
