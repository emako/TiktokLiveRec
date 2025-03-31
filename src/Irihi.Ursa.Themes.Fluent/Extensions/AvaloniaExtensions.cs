using Avalonia;

namespace UrsaAvaloniaUI.Extensions;

public static class AvaloniaExtensions
{
    public static T? FindParentOfType<T>(this StyledElement element) where T : class
    {
        StyledElement? parent = element.Parent;

        while (parent != null)
        {
            if (parent is T target)
                return target;
            parent = parent?.Parent;
        }
        return null;
    }
}
