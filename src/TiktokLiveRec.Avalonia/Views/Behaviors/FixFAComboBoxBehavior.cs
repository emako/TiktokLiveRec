using Avalonia.Xaml.Interactivity;
using FluentAvalonia.UI.Controls;

namespace TiktokLiveRec.Views.Behaviors;

/// <summary>
/// Fix something only avalonia can do
/// </summary>
public sealed class FixFAComboBoxBehavior : Behavior<FAComboBox>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        Locale.CultureChanged += OnLocaleChanged;
    }

    protected override void OnDetaching()
    {
        Locale.CultureChanged -= OnLocaleChanged;
        base.OnDetaching();
    }

    private void OnLocaleChanged(object? sender, EventArgs e)
    {
        if (AssociatedObject?.SelectedItem is not null)
        {
            // SelectingItemsControl.SelectedItemProperty will not be notified when locale changed
            // https://github.com/AvaloniaUI/Avalonia/discussions/15767#discussioncomment-12248981
            object? prev = AssociatedObject.SelectedItem;
            AssociatedObject.SelectedItem = null;
            AssociatedObject.SelectedItem = prev;
        }
    }
}
