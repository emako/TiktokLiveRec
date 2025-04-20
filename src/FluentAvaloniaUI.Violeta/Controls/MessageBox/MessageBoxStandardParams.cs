namespace FluentAvalonia.UI.Controls;

public class MessageBoxStandardParams : AbstractMessageBoxParams
{
    /// <summary>
    /// Icon of window
    /// </summary>
    public MessageBoxImage Icon { get; set; } = MessageBoxImage.None;

    /// <summary>
    /// Default buttons
    /// </summary>
    public MessageBoxButton ButtonDefinitions { get; set; } = MessageBoxButton.OK;

    public MessageBoxResult EnterDefaultButton { get; set; } = MessageBoxResult.Default;
    public MessageBoxResult EscDefaultButton { get; set; } = MessageBoxResult.Default;
}
