using CommunityToolkit.Mvvm.ComponentModel;
using ComputedConverters;
using Fischless.Configuration;
using Wpf.Ui.Appearance;
using Wpf.Ui.Violeta.Appearance;

namespace TiktokLiveRec.ViewModels;

[ObservableObject]
public partial class SettingsViewModel : ReactiveObject
{
    [ObservableProperty]
    private int themeIndex = Configurations.Theme.Get() switch
    {
        nameof(ApplicationTheme.Light) => (int)ApplicationTheme.Light,
        nameof(ApplicationTheme.Dark) => (int)ApplicationTheme.Dark,
        _ => (int)ApplicationTheme.Unknown,
    };

    partial void OnThemeIndexChanged(int value)
    {
        ThemeManager.Apply((ApplicationTheme)value);
        Configurations.Theme.Set((ApplicationTheme)value switch
        {
            ApplicationTheme.Light => nameof(ApplicationTheme.Light),
            ApplicationTheme.Dark => nameof(ApplicationTheme.Dark),
            _ => string.Empty,
        });
        ConfigurationManager.Save();
    }
}
