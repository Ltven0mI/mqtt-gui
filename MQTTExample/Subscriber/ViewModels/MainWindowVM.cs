using CommonClasses;
using MVVMUtil;
using Subscriber.Services.Interfaces;
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
          SubscriberService.Connect(Address, Port);
        },
        _ => SubscriberService.ConnectionState == ClientConnectionState.Disconnected
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
          SubscriberService.Disconnect();
        },
        _ => SubscriberService.ConnectionState != ClientConnectionState.Disconnected
      );
    }
    #endregion Command - StopCommand

    #region Command - SubscribeCommand
    private RelayCommand _subscribeCommand;
    public ICommand SubscribeCommand
    {
      get => _subscribeCommand ??= new RelayCommand(
        _ =>
        {
          // Sanitize Topic.
          Topic = Topic?.Trim();

          // Ensure Topic is Valid //
          if (string.IsNullOrWhiteSpace(Topic))
          {
            MessageBox.Show("Please enter a valid topic.", "Invalid Topic");
            return;
          }

          SubscriberService.Subscribe(Topic);
          Topic = "";
        },
        _ => SubscriberService.ConnectionState == ClientConnectionState.Connected
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
          // Sanitize Topic.
          Topic = Topic?.Trim();

          // Ensure Topic is Valid //
          if (string.IsNullOrWhiteSpace(Topic))
          {
            MessageBox.Show("Please enter a valid topic.", "Invalid Topic");
            return;
          }

          SubscriberService.Unsubscribe(Topic);
          Topic = "";
        },
        _ => SubscriberService.ConnectionState == ClientConnectionState.Connected
      );
    }
    #endregion UnsubscribeCommand

    #endregion Commands


    #region Models
    #endregion Models


    #region Services

    #region Service - SubscriberService
    private ISubscriberService _subscriberService;
    [Unity.Dependency]
    public ISubscriberService SubscriberService
    {
      get => _subscriberService;
      set
      {
        // Deregister Callbacks
        if (_subscriberService != null)
        {
          _subscriberService.ConnectionSuccessful -= SubscriberService_ConnectionSuccessful;
          _subscriberService.ConnectionCancelled -= SubscriberService_ConnectionCancelled;
          _subscriberService.ConnectionAttemptFailed -= SubscriberService_ConnectionAttemptFailed;
          _subscriberService.ConnectionFailed -= SubscriberService_ConnectionFailed;
          _subscriberService.ConnectionLost -= SubscriberService_ConnectionLost;
          _subscriberService.ConnectionClosed -= SubscriberService_ConnectionClosed;
          _subscriberService.Subscribed -= SubscriberService_Subscribed;
          _subscriberService.Unsubscribed -= SubscriberService_Unsubscribed;
          _subscriberService.MessageReceived -= SubscriberService_MessageReceived;
        }

        _subscriberService = value;

        // Register Callbacks
        if (_subscriberService != null)
        {
          _subscriberService.ConnectionSuccessful += SubscriberService_ConnectionSuccessful;
          _subscriberService.ConnectionCancelled += SubscriberService_ConnectionCancelled;
          _subscriberService.ConnectionAttemptFailed += SubscriberService_ConnectionAttemptFailed;
          _subscriberService.ConnectionFailed += SubscriberService_ConnectionFailed;
          _subscriberService.ConnectionLost += SubscriberService_ConnectionLost;
          _subscriberService.ConnectionClosed += SubscriberService_ConnectionClosed;
          _subscriberService.Subscribed += SubscriberService_Subscribed;
          _subscriberService.Unsubscribed += SubscriberService_Unsubscribed;
          _subscriberService.MessageReceived += SubscriberService_MessageReceived;
        }
      }
    }
    #endregion Service - SubscriberService

    #endregion Services


    #region Callback Handlers

    #region Source Group - SubscriberService
    private void SubscriberService_ConnectionSuccessful(object sender, EventArgs e)
    {
      LogText += $"[SYS] Connection Successful!{Environment.NewLine}";
    }
    private void SubscriberService_ConnectionCancelled(object sender, EventArgs e)
    {
      LogText += $"[SYS] Connection Cancelled!{Environment.NewLine}";
    }
    private void SubscriberService_ConnectionAttemptFailed(object sender, EventArgs e)
    {
      LogText += $"[SYS] Connection Attempt Failed!{Environment.NewLine}";
    }
    private void SubscriberService_ConnectionFailed(object sender, EventArgs e)
    {
      LogText += $"[SYS] Connection Failed!{Environment.NewLine}";
    }
    private void SubscriberService_ConnectionLost(object sender, EventArgs e)
    {
      LogText += $"[SYS] Connection Lost!{Environment.NewLine}";
    }
    private void SubscriberService_ConnectionClosed(object sender, EventArgs e)
    {
      LogText += $"[SYS] Connection Closed!{Environment.NewLine}";
    }
    private void SubscriberService_Subscribed(object sender, SubscriptionEventArgs e)
    {
      LogText += $"[SUB] '{e.Topic}' {Environment.NewLine}";
    }
    private void SubscriberService_Unsubscribed(object sender, SubscriptionEventArgs e)
    {
      LogText += $"[UNSUB] '{e.Topic}' {Environment.NewLine}";
    }
    private void SubscriberService_MessageReceived(object sender, MessageReceivedEventArgs e)
    {
      LogText += $"[{e.Topic}] '{e.Payload}' {Environment.NewLine}";
    }
    #endregion Source Group - SubscriberService

    #endregion Callback Handlers
  }
}
