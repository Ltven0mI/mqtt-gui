﻿using Subscriber.ViewModels;
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

      // Create Container
      var container = new UnityContainer();

      // Register Services

      // Register ViewModels
      container.RegisterType<IViewMainWindowVM, MainWindowVM>();

      // Create MainView
      var view = container.Resolve<MainWindowView>();

      // Show the MainView
      view.Show();
    }
  }
}
