namespace FluentAvalonia.UI.Violeta.Platform.Windows.Dialogs.CommonFileDialogs;

#pragma warning disable CS8618

public static class CommonFileDialogStandardFilters
{
    private static CommonFileDialogFilter officeFilesFilter;
    private static CommonFileDialogFilter pictureFilesFilter;
    private static CommonFileDialogFilter textFilesFilter;

    public static CommonFileDialogFilter OfficeFiles
    {
        get
        {
            officeFilesFilter ??= new CommonFileDialogFilter(LocalizedMessages.CommonFiltersOffice,
                    "*.doc, *.docx, *.xls, *.xlsx, *.ppt, *.pptx");
            return officeFilesFilter;
        }
    }

    public static CommonFileDialogFilter PictureFiles
    {
        get
        {
            pictureFilesFilter
                ??= new CommonFileDialogFilter(LocalizedMessages.CommonFiltersPicture,
                    "*.bmp, *.jpg, *.jpeg, *.png, *.ico");
            return pictureFilesFilter;
        }
    }

    public static CommonFileDialogFilter TextFiles
    {
        get
        {
            textFilesFilter
                ??= new CommonFileDialogFilter(LocalizedMessages.CommonFiltersText, "*.txt");
            return textFilesFilter;
        }
    }
}
