using Fischless.Configuration;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using TiktokLiveRec.Core;
using TiktokLiveRec.Extensions;
using Wpf.Ui.Violeta.Controls;

namespace TiktokLiveRec;

public partial class App : Application
{
    static App()
    {
        ConfigurationManager.ConfigurationSerializer = new YamlConfigurationSerializer();
        ConfigurationManager.Setup(ConfigurationSpecialPath.GetPath("config.yaml", AppConfig.PackName));
    }

    public App()
    {
        InitializeComponent();

        DispatcherUnhandledException += (object s, DispatcherUnhandledExceptionEventArgs e) =>
        {
            e.Handled = true;
            ExceptionReport.Show(e.Exception);
        };
    }

    /// <summary>
    /// Occurs when the application is loading.
    /// </summary>
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        RuntimeHelper.CheckSingleInstance(AppConfig.PackName + (Debugger.IsAttached ? "_DEBUG" : string.Empty));
        TrayIconManager.Start();
    }

    /// <summary>
    /// Occurs when the application is closing.
    /// </summary>
    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
    }
}
