using Avalonia.Controls;
using Avalonia.Media;
using System.ComponentModel;

namespace FluentAvalonia.UI.Controls;

public abstract class AbstractMessageBoxViewModel
{
    protected AbstractMessageBoxViewModel(AbstractMessageBoxParams @params, MessageBoxImage icon = MessageBoxImage.None)
    {
        ImageGlyph = icon switch
        {
            MessageBoxImage.None => null!,
            MessageBoxImage.Error => FontSymbols.Error,
            MessageBoxImage.Information => FontSymbols.Info,
            MessageBoxImage.Warning => FontSymbols.Warning,
            MessageBoxImage.Question => FontSymbols.StatusCircleQuestionMark,
            _ => throw new NotSupportedException(),
        };

        MinWidth = @params.MinWidth;
        MaxWidth = @params.MaxWidth;
        Width = @params.Width;
        MinHeight = @params.MinHeight;
        MaxHeight = @params.MaxHeight;
        Height = @params.Height;
        CanResize = @params.CanResize;
        FontFamily = @params.FontFamily;
        ContentTitle = @params.ContentTitle;
        ContentHeader = @params.ContentHeader;
        ContentMessage = @params.ContentMessage;
        Markdown = @params.Markdown;
        WindowIconPath = @params.WindowIcon;
        SizeToContent = @params.SizeToContent;
        LocationOfMyWindow = @params.WindowStartupLocation;
        SystemDecorations = @params.SystemDecorations;
        Topmost = @params.Topmost;

        if (@params.InputParams != null)
        {
            InputLabel = @params.InputParams.Label;
            InputValue = @params.InputParams.DefaultValue;
            IsInputMultiline = @params.InputParams.Multiline;
            IsInputVisible = true;
        }
    }

    public bool CanResize { get; }
    public bool HasHeader => !string.IsNullOrEmpty(ContentHeader);
    public bool HasIcon => ImageGlyph is not null;
    public FontFamily FontFamily { get; }
    public string ContentTitle { get; }
    public string ContentHeader { get; }
    public string ContentMessage { get; set; }
    public bool Markdown { get; set; }
    public WindowIcon WindowIconPath { get; }
    public string ImageGlyph { get; }
    public double MinWidth { get; set; }
    public double MaxWidth { get; set; }
    public double Width { get; set; }

    public double MinHeight { get; set; }
    public double MaxHeight { get; set; }
    public double Height { get; set; }

    public SystemDecorations SystemDecorations { get; set; }
    public bool Topmost { get; set; }

    public SizeToContent SizeToContent { get; set; } = SizeToContent.Height;

    public WindowStartupLocation LocationOfMyWindow { get; }

    public abstract string InputLabel { get; internal set; }
    public abstract string InputValue { get; set; }
    public abstract bool IsInputMultiline { get; internal set; }
    public abstract bool IsInputVisible { get; internal set; }
}
