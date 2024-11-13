using Antelcat.I18N.Attributes;
using System.Globalization;
using System.Windows;
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

    private static void SetCulture(CultureInfo value)
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

[ResourceKeysOf(typeof(Resources))]
public partial class LangKeys;
