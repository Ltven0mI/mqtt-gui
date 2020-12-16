using MVVMUtil;
using Publisher.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Publisher.ViewModels
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

    #region Property - Content
    private string _content;
    public string Content
    {
      get => _content;
      set
      {
        _content = value;
        RaisePropertyChanged(nameof(Content));
      }
    }
    #endregion Property - Content

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

    #region Command - PublishCommand
    private RelayCommand _publishCommand;
    public ICommand PublishCommand
    {
      get => _publishCommand ??= new RelayCommand(
        _ =>
        {
          MessageBox.Show("Publish");
        }
      );
    }
    #endregion Command - PublishCommand

    #endregion Commands

    #region Models
    #endregion Models

    #region Services
    #endregion Services

  }
}
