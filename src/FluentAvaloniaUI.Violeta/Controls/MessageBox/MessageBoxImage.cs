namespace FluentAvalonia.UI.Controls;

/// <summary>
/// Specifies the icon that is displayed by a message box.
/// </summary>
public enum MessageBoxImage
{
    //
    // Summary:
    //     The message box contains no symbols.
    None = 0,

    //
    // Summary:
    //     The message box contains a symbol consisting of white X in a circle with a red
    //     background.
    Error = 16,

    //
    // Summary:
    //     The message box contains a symbol consisting of a white X in a circle with a
    //     red background.
    Hand = 16,

    //
    // Summary:
    //     The message box contains a symbol consisting of white X in a circle with a red
    //     background.
    Stop = 16,

    //
    // Summary:
    //     The message box contains a symbol consisting of a question mark in a circle.
    //     The question mark message icon is no longer recommended because it does not clearly
    //     represent a specific type of message and because the phrasing of a message as
    //     a question could apply to any message type. In addition, users can confuse the
    //     question mark symbol with a help information symbol. Therefore, do not use this
    //     question mark symbol in your message boxes. The system continues to support its
    //     inclusion only for backward compatibility.
    Question = 32,

    //
    // Summary:
    //     The message box contains a symbol consisting of an exclamation point in a triangle
    //     with a yellow background.
    Exclamation = 48,

    //
    // Summary:
    //     The message box contains a symbol consisting of an exclamation point in a triangle
    //     with a yellow background.
    Warning = 48,

    //
    // Summary:
    //     The message box contains a symbol consisting of a lowercase letter i in a circle.
    Asterisk = 64,

    //
    // Summary:
    //     The message box contains a symbol consisting of a lowercase letter i in a circle.
    Information = 64
}
