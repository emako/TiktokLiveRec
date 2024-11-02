using Fischless.Configuration;
using System.Diagnostics;
using System.Windows;
using TiktokLiveRec.Core;
using TiktokLiveRec.Extensions;

namespace TiktokLiveRec;

public partial class App : Application
{
    static App()
    {
        ConfigurationManager.ConfigurationSerializer = new YamlConfigurationSerializer();
        ConfigurationManager.Setup(ConfigurationSpecialPath.GetPath("config.yaml", AppConfig.PackName));
    }

    /// <summary>
    /// Occurs when the application is loading.
    /// </summary>
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        RuntimeHelper.CheckSingleInstance(AppConfig.PackName + (Debugger.IsAttached ? "_DEBUG" : string.Empty));
        SpiderResult result = Spider.GetResult("https://live.douyin.com/940169270753");
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
