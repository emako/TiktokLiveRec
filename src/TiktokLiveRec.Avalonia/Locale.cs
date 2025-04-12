using Avalonia.Markup.Xaml.MarkupExtensions;

using System.Globalization;

using TiktokLiveRec.Properties;

namespace TiktokLiveRec;

internal static class Locale
{
    public static event EventHandler? CultureChanged;

    public static CultureInfo Fallback { get; } = new CultureInfo("en-US");

    public static CultureInfo Culture
    {
        get => CultureInfo.CurrentUICulture;
        set => SetCulture(value);
    }

    private static void SetCulture(CultureInfo? value)
    {
        CultureInfo culture = value ?? Fallback;

        while (Resources.ResourceManager.GetResourceSet(culture, true, false) is null)
        {
            if (culture.Parent == CultureInfo.InvariantCulture)
            {
                culture = Fallback;
                break;
            }
            culture = culture.Parent;
        }

        I18NExtension.Culture
            = CultureInfo.CurrentCulture
            = CultureInfo.CurrentUICulture
            = culture;

        CultureChanged?.Invoke(CultureChanged.Target, EventArgs.Empty);
    }
}

internal static class LocaleExtension
{
    public static string Tr(this string key)
    {
        try
        {
            return I18NExtension.Translate(key) ?? string.Empty;
        }
        catch (Exception e)
        {
            _ = e;
        }
        return null!;
    }

    public static string Tr(this string key, params object[] args)
    {
        return string.Format(Tr(key)?.ToString() ?? string.Empty, args);
    }
}

internal static class CultureInfoExtension
{
    public static bool IsUseWordSpace(this CultureInfo culture)
    {
        string language = culture.TwoLetterISOLanguageName;
        return !Array.Exists(["zh", "ja", "ko"], lang => lang == language);
    }

    public static string WordSpace(this CultureInfo culture)
    {
        return culture.IsUseWordSpace() ? " " : string.Empty;
    }

    public static bool IsUseFullWidth(this CultureInfo culture)
    {
        string language = culture.TwoLetterISOLanguageName;
        return !Array.Exists(["zh", "ja", "ko"], lang => lang == language);
    }

    public static string SymbolWidth(this string input, CultureInfo culture)
    {
        bool isFullWidthCulture = culture.IsUseFullWidth();
        char[] result = input.ToCharArray();

        for (int i = 0; i < result.Length; i++)
        {
            if (isFullWidthCulture)
            {
                if (result[i] >= 0x21 && result[i] <= 0x7E)
                {
                    result[i] = (char)(result[i] + 0xFEE0);
                }
            }
            else
            {
                if (result[i] >= 0xFF01 && result[i] <= 0xFF5E)
                {
                    result[i] = (char)(result[i] - 0xFEE0);
                }
            }
        }

        return new string(result);
    }
}
