using Avalonia.Controls;
using Avalonia.Markup.Xaml.Styling;

namespace AvaloniaUI.Violeta.Resources;

public sealed class ControlsResources : ResourceDictionary
{
    public ControlsResources()
    {
        MergedDictionaries.Add(new ResourceInclude(new Uri("avares://AvaloniaUI.Violeta"))
        {
            Source = new Uri("/Resources/Resources.axaml", UriKind.Relative),
        });
    }
}
