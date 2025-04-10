using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Metadata;

namespace AvaloniaUI.Violeta.Controls;

[TemplatePart(PART_ContentPresenter, typeof(ContentPresenter))]
public class Card : ContentControl
{
    private const string PART_ContentPresenter = "PART_ContentPresenter";

    public new static readonly StyledProperty<object?> ContentProperty =
        AvaloniaProperty.Register<Card, object?>(nameof(Content));

    [Content]
    [DependsOn(nameof(ContentTemplate))]
    public new object? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }
}
