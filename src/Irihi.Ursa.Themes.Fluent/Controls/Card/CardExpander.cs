using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Metadata;
using FluentAvalonia.UI.Controls;
using System.Diagnostics.CodeAnalysis;

namespace UrsaAvaloniaUI.Controls;

[TemplatePart(PART_HeaderContentPresenter, typeof(ContentPresenter))]
[TemplatePart(PART_CardContentPresenter, typeof(ContentPresenter))]
[TemplatePart(PART_HeaderContentBorder, typeof(FABorder))]
[TemplatePart(PART_CardContentBorder, typeof(FABorder))]
[SuppressMessage("CodeQuality", "IDE0052:Remove unread private members")]
public class CardExpander : ContentControl
{
    private const string PART_HeaderContentPresenter = "PART_HeaderContentPresenter";
    private const string PART_CardContentPresenter = "PART_CardContentPresenter";
    private const string PART_HeaderContentBorder = "PART_HeaderContentBorder";
    private const string PART_CardContentBorder = "PART_CardContentBorder";

    public static readonly StyledProperty<bool> IsExpandedProperty =
        AvaloniaProperty.Register<CardExpander, bool>(nameof(IsExpanded), defaultValue: false);

    public static readonly StyledProperty<object?> HeaderContentProperty =
        AvaloniaProperty.Register<CardExpander, object?>(nameof(HeaderContent));

    public static readonly StyledProperty<object?> CardContentProperty =
        AvaloniaProperty.Register<CardExpander, object?>(nameof(CardContent));

    private ContentPresenter? _headerContentPresenter;
    private ContentPresenter? _cardContentPresenter;
    private FABorder? _headerContentBorder;
    private FABorder? _cardContentBorder;

    public bool IsExpanded
    {
        get => GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    public object? HeaderContent
    {
        get => GetValue(HeaderContentProperty);
        set => SetValue(HeaderContentProperty, value);
    }

    [Content]
    [DependsOn(nameof(ContentTemplate))]
    public object? CardContent
    {
        get => GetValue(CardContentProperty);
        set => SetValue(CardContentProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _headerContentPresenter = e.NameScope.Find<ContentPresenter>(PART_HeaderContentPresenter);
        _cardContentPresenter = e.NameScope.Find<ContentPresenter>(PART_CardContentPresenter);
        _headerContentBorder = e.NameScope.Find<FABorder>(PART_HeaderContentBorder);
        _cardContentBorder = e.NameScope.Find<FABorder>(PART_CardContentBorder);

        if (_headerContentBorder != null)
        {
            _headerContentBorder.Tapped += OnHeaderContentTapped;
        }

        UpdateThickness();
    }

    protected virtual void OnHeaderContentTapped(object? sender, TappedEventArgs e)
    {
        IsExpanded = !IsExpanded;

        UpdateThickness();
    }

    protected void UpdateThickness()
    {
        if (this.FindResource("ExpanderHeaderBorderThickness") is Thickness thickness
            && this.FindResource("ControlCornerRadius") is CornerRadius cornerRadius)
        {
            double t = thickness.Left;
            double c = cornerRadius.TopRight;

            if (IsExpanded)
            {
                _headerContentBorder!.BorderThickness = new Thickness(t, t, t, t);
                _headerContentBorder!.CornerRadius = new CornerRadius(c, c, 0, 0);
                _cardContentBorder!.BorderThickness = new Thickness(t, 0, t, t);
                _cardContentBorder!.CornerRadius = new CornerRadius(0, 0, c, c);
            }
            else
            {
                _headerContentBorder!.BorderThickness = new Thickness(t, t, t, t);
                _headerContentBorder!.CornerRadius = new CornerRadius(c, c, c, c);
                _cardContentBorder!.BorderThickness = new Thickness(0);
                _cardContentBorder!.CornerRadius = new CornerRadius(0);
            }
        }
    }
}
