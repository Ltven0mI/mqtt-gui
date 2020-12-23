using CommonClasses;
using MVVMUtil;
using Publisher.Services.Interfaces;
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

    #region Property - Payload
    private string _payload;
    public string Payload
    {
      get => _payload;
      set
      {
        _payload = value;
        RaisePropertyChanged(nameof(Payload));
      }
    }
    #endregion Property - Payload

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

    #region Command - StartCommand
    private RelayCommand _startCommand;
    public ICommand StartCommand
    {
      get => _startCommand ??= new RelayCommand(
        _ =>
        {
          // Sanitize Address
          Address = Address?.Trim();

          // If Address Empty: Inform User, Return
          if (string.IsNullOrWhiteSpace(Address))
          {
            MessageBox.Show("Please enter a valid address, then try again.", "Invalid Address");
            return;
          }

          // Attempt Connection
          PublisherService.Connect(Address, Port);
        },
        _ => PublisherService.ConnectionState == ClientConnectionState.Disconnected
      );
    }
    #endregion Command - StartCommand

    #region Command - StopCommand
    private RelayCommand _stopCommand;
    public ICommand StopCommand
    {
      get => _stopCommand ??= new RelayCommand(
        _ =>
        {
          PublisherService.Disconnect();
        },
        _ => PublisherService.ConnectionState != ClientConnectionState.Disconnected
      );
    }
    #endregion Command - StopCommand

    #region Command - PublishCommand
    private RelayCommand _publishCommand;
    public ICommand PublishCommand
    {
      get => _publishCommand ??= new RelayCommand(
        _ =>
        {
          // Input Sanitizing //
          Topic = Topic?.Trim();
          Payload = Payload?.Trim();

          // Input Validation //
          if (string.IsNullOrWhiteSpace(Topic))
          {
            MessageBox.Show("Please enter a valid topic.", "Invalid Topic");
            return;
          }
          if(string.IsNullOrWhiteSpace(Payload))
          {
            MessageBox.Show("Please enter a valid content.", "Invalid Payload");
            return;
          }

          PublisherService.Publish(Topic, Payload);
          Payload = "";
        },
        _ => PublisherService.ConnectionState == ClientConnectionState.Connected
      );
    }
    #endregion Command - PublishCommand

    #endregion Commands


    #region Models
    #endregion Models


    #region Services

    #region Service - PublisherService
    private IPublisherService _publisherService;
    [Unity.Dependency]
    public IPublisherService PublisherService
    {
      get => _publisherService;
      set
      {
        // Deregister Callbacks
        if (_publisherService != null)
        {
          _publisherService.ConnectionSuccessful -= PublisherService_ConnectionSuccessful;
          _publisherService.ConnectionCancelled -= PublisherService_ConnectionCancelled;
          _publisherService.ConnectionAttemptFailed -= PublisherService_ConnectionAttemptFailed;
          _publisherService.ConnectionFailed -= PublisherService_ConnectionFailed;
          _publisherService.ConnectionLost -= PublisherService_ConnectionLost;
          _publisherService.ConnectionClosed -= PublisherService_ConnectionClosed;
          _publisherService.MessagePublished -= PublisherService_MessagePublished;
        }

        _publisherService = value;

        // Register Callbacks
        if (_publisherService != null)
        {
          _publisherService.ConnectionSuccessful += PublisherService_ConnectionSuccessful;
          _publisherService.ConnectionCancelled += PublisherService_ConnectionCancelled;
          _publisherService.ConnectionAttemptFailed += PublisherService_ConnectionAttemptFailed;
          _publisherService.ConnectionFailed += PublisherService_ConnectionFailed;
          _publisherService.ConnectionLost += PublisherService_ConnectionLost;
          _publisherService.ConnectionClosed += PublisherService_ConnectionClosed;
          _publisherService.MessagePublished += PublisherService_MessagePublished;
        }
      }
    }
    #endregion Service - PublisherService

    #endregion Services


    #region Callback Handlers

    #region Source Group - PublisherService
    private void PublisherService_ConnectionSuccessful(object sender, EventArgs e)
    {
      LogText += $"[SYS] Connection Successful!{Environment.NewLine}";
    }
    private void PublisherService_ConnectionCancelled(object sender, EventArgs e)
    {
      LogText += $"[SYS] Connection Cancelled!{Environment.NewLine}";
    }
    private void PublisherService_ConnectionAttemptFailed(object sender, EventArgs e)
    {
      LogText += $"[SYS] Connection Attempt Failed!{Environment.NewLine}";
    }
    private void PublisherService_ConnectionFailed(object sender, EventArgs e)
    {
      LogText += $"[SYS] Connection Failed!{Environment.NewLine}";
    }
    private void PublisherService_ConnectionLost(object sender, EventArgs e)
    {
      LogText += $"[SYS] Connection Lost!{Environment.NewLine}";
    }
    private void PublisherService_ConnectionClosed(object sender, EventArgs e)
    {
      LogText += $"[SYS] Connection Closed!{Environment.NewLine}";
    }
    private void PublisherService_MessagePublished(object sender, MessagePublishedEventArgs e)
    {
      LogText += $"[{e.Topic}] Published: '{e.Payload}'{Environment.NewLine}";
    }
    #endregion Source Group - PublisherService

    #endregion Callback Handlers
  }
}
