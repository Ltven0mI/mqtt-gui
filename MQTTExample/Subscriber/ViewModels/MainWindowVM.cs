using MVVMUtil;
using Subscriber.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Subscriber.ViewModels
{
  public class MainWindowVM : ViewModelBase, IViewMainWindowVM
  {
    #region Properties

    #region Property - Address
    private string _address;
    public string Address
    {
      get => _address;
      set
      {
        _address = value;
        RaisePropertyChanged(nameof(Address));
      }
    }
    #endregion Property - Address

    #region Property - Port
    private ushort _port;
    public ushort Port
    {
      get => _port;
      set
      {
        _port = value;
        RaisePropertyChanged(nameof(Port));
      }
    }
    #endregion Property - Port

    #region Property - Topic
    private string _topic;
    public string Topic
    {
      get => _topic;
      set
      {
        _topic = value;
        RaisePropertyChanged(nameof(Topic));
      }
    }
    #endregion Property - Topic

    #region Property - LogText
    private string _logText;
    public string LogText
    {
      get => _logText;
      set
      {
        _logText = value;
        RaisePropertyChanged(nameof(LogText));
      }
    }
    #endregion Property - LogText

    #endregion Properties

    #region Commands

    #region Command - StartStopCommand
    private RelayCommand _startStopCommand;
    public ICommand StartStopCommand
    {
      get => _startStopCommand ??= new RelayCommand(
        _ =>
        {
          MessageBox.Show("Start / Stop");
        }
      );
    }
    #endregion Command - StartStopCommand

    #region Command - SubscribeCommand
    private RelayCommand _subscribeCommand;
    public ICommand SubscribeCommand
    {
      get => _subscribeCommand ??= new RelayCommand(
        _ =>
        {
          MessageBox.Show("Subscribe");
        }
      );
    }
    #endregion Command - SubscribeCommand

    #region Command - UnsubscribeCommand
    private RelayCommand _unsubscribeCommand;
    public ICommand UnsubscribeCommand
    {
      get => _unsubscribeCommand ??= new RelayCommand(
        _ =>
        {
          MessageBox.Show("Unsubscribe");
        }
      );
    }
    #endregion UnsubscribeCommand

    #endregion Commands

    #region Models
    #endregion Models

    #region Services
    #endregion Services
  }
}
