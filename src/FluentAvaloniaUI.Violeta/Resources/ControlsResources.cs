using Avalonia.Controls;
using Avalonia.Markup.Xaml.Styling;

namespace FluentAvalonia.UI.Violeta.Resources;

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
