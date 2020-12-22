using Subscriber.Services;
using Subscriber.Services.Interfaces;
using Subscriber.ViewModels;
using Subscriber.ViewModels.Interfaces;
using Subscriber.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using Serilog;
using Serilog.Sinks.File;
using Serilog.Sinks.SystemConsole;

namespace Subscriber
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

      // Initialize Serilog
      Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .WriteTo.File("logs\\subscriber.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();

      // Create Container
      var container = new UnityContainer();

      // Register Services
      container.RegisterType<ISubscriberService, SubscriberService>();

      // Register ViewModels
      container.RegisterType<IViewMainWindowVM, MainWindowVM>();

      // Create MainView
      var view = container.Resolve<MainWindowView>();

      // Show the MainView
      view.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
      base.OnExit(e);

      Log.CloseAndFlush();
    }
  }
}
