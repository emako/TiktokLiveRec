using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace FluentAvalonia.UI.Controls;

public class PersonPicture : Button
{
    public static readonly StyledProperty<IImage?> SourceProperty =
        AvaloniaProperty.Register<PersonPicture, IImage?>(nameof(Source));

    public static readonly StyledProperty<object?> HoverMaskProperty =
        AvaloniaProperty.Register<PersonPicture, object?>(nameof(HoverMask));

    public IImage? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public object? HoverMask
    {
        get => GetValue(HoverMaskProperty);
        set => SetValue(HoverMaskProperty, value);
    }
}
