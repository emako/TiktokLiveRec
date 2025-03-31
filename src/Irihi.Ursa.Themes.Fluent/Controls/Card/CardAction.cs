using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Metadata;
using System.Windows.Input;

namespace UrsaAvaloniaUI.Controls;

[TemplatePart("PART_ContentPresenter", typeof(ContentPresenter))]
public class CardAction : ContentControl
{
    public new static readonly StyledProperty<object?> ContentProperty = AvaloniaProperty.Register<CardAction, object?>(nameof(Content));

    [Content]
    [DependsOn("ContentTemplate")]
    public new object? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public static readonly StyledProperty<ICommand?> CommandProperty = AvaloniaProperty.Register<CardAction, ICommand>(nameof(Command), null!, inherits: false, BindingMode.OneWay, null, null, enableDataValidation: true)!;

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly StyledProperty<object?> CommandParameterProperty = AvaloniaProperty.Register<CardAction, object>(nameof(CommandParameter))!;

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public CardAction()
    {
        AddHandler(PointerPressedEvent, OnPointerPressed, RoutingStrategies.Tunnel);
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (Command?.CanExecute(CommandParameter) == true)
        {
            Command.Execute(CommandParameter);
        }
    }
}
