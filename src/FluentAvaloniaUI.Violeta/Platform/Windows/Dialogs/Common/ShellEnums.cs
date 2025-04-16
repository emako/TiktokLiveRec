using System.Runtime.Versioning;

namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.Common;

[SupportedOSPlatform("Windows")]
public enum DisplayNameType
{
    Default = 0x00000000,
    RelativeToParent = unchecked((int)0x80018001),
    RelativeToParentAddressBar = unchecked((int)0x8007c001),
    RelativeToDesktop = unchecked((int)0x80028000),
    RelativeToParentEditing = unchecked((int)0x80031001),
    RelativeToDesktopEditing = unchecked((int)0x8004c000),
    FileSystemPath = unchecked((int)0x80058000),
    Url = unchecked((int)0x80068000),
}

[SupportedOSPlatform("Windows")]
public enum FileDialogAddPlaceLocation
{
    Bottom = 0x00000000,
    Top = 0x00000001,
}

[SupportedOSPlatform("Windows")]
public enum FolderLogicalViewMode
{
    Unspecified = -1,
    None = 0,
    First = 1,
    Details = 1,
    Tiles = 2,
    Icons = 3,
    List = 4,
    Content = 5,
    Last = 5,
}

[SupportedOSPlatform("Windows")]
public enum LibraryFolderType
{
    Generic = 0,
    Documents,
    Music,
    Pictures,
    Videos,
}

[SupportedOSPlatform("Windows")]
public enum QueryParserManagerOption
{
    SchemaBinaryName = 0,
    PreLocalizedSchemaBinaryPath = 1,
    UnlocalizedSchemaBinaryPath = 2,
    LocalizedSchemaBinaryPath = 3,
    AppendLCIDToLocalizedPath = 4,
    LocalizerSupport = 5,
}

[SupportedOSPlatform("Windows")]
public enum SearchConditionOperation
{
    Implicit = 0,
    Equal = 1,
    NotEqual = 2,
    LessThan = 3,
    GreaterThan = 4,
    LessThanOrEqual = 5,
    GreaterThanOrEqual = 6,
    ValueStartsWith = 7,
    ValueEndsWith = 8,
    ValueContains = 9,
    ValueNotContains = 10,
    DosWildcards = 11,
    WordEqual = 12,
    WordStartsWith = 13,
    ApplicationSpecific = 14,
}

[SupportedOSPlatform("Windows")]
public enum SearchConditionType
{
    And = 0,
    Or = 1,
    Not = 2,
    Leaf = 3,
}

[SupportedOSPlatform("Windows")]
public enum SortDirection
{
    Default = 0,
    Descending = -1,
    Ascending = 1,
}

[SupportedOSPlatform("Windows")]
public enum StructuredQueryMultipleOption
{
    VirtualProperty,
    DefaultProperty,
    GeneratorForType,
    MapProperty,
}

[SupportedOSPlatform("Windows")]
public enum StructuredQuerySingleOption
{
    Schema,
    Locale,
    WordBreaker,
    NaturalSyntax,
    AutomaticWildcard,
    TraceLevel,
    LanguageKeywords,
    Syntax,
    TimeZone,
    ImplicitConnector,
    ConnectorCase,
}

[SupportedOSPlatform("Windows")]
public enum WindowShowCommand
{
    Hide = 0,
    Normal = 1,
    Minimized = 2,
    Maximized = 3,
    ShowNoActivate = 4,
    Show = 5,
    Minimize = 6,
    ShowMinimizedNoActivate = 7,
    ShowNA = 8,
    Restore = 9,
    Default = 10,
    ForceMinimize = 11,
}
