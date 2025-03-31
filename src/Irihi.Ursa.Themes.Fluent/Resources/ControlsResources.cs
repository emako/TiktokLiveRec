using Avalonia.Controls;
using Avalonia.Markup.Xaml.Styling;

namespace UrsaAvaloniaUI.Resources;

public sealed class ControlsResources : ResourceDictionary
{
    public ControlsResources()
    {
        MergedDictionaries.Add(new ResourceInclude(new Uri("avares://UrsaAvaloniaUI"))
        {
            Source = new Uri("/Resources/Resources.axaml", UriKind.Relative),
        });
    }
}
