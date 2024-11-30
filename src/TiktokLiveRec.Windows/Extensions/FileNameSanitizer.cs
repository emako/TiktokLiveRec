using System.IO;
using Windows.Foundation.Metadata;

namespace TiktokLiveRec.Extensions;

[Platform(Platform.Windows)]
internal static class FileNameSanitizer
{
    /// <summary>
    /// Sanitizes the input file name by replacing invalid characters with underscores.
    /// </summary>
    /// <param name="fileName">The input file name.</param>
    /// <returns>The sanitized file name.</returns>
    public static string SanitizeFileName(this string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name cannot be null or whitespace.", nameof(fileName));

        // Get invalid file name characters.
        char[] invalidChars = Path.GetInvalidFileNameChars();

        // Replace each invalid character with an underscore.
        return string.Concat(fileName.Select(ch => invalidChars.Contains(ch) ? '_' : ch));
    }

    /// <summary>
    /// Replaces all trailing periods (.) in a string with underscores (_).
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <returns>The modified string with trailing periods replaced by underscores.</returns>
    public static string ReplaceTrailingDotsWithUnderscores(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        int i = input.Length - 1;
        while (i >= 0 && input[i] == '.')
        {
            i--;
        }

        // Replace trailing dots with underscores
        return string.Concat(input.AsSpan(0, i + 1), new string('_', input.Length - i - 1));
    }
}
