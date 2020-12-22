using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClasses.Services.Interfaces
{
  public interface IClientService
  {
    public event EventHandler ConnectionSuccessful;
    public event EventHandler ConnectionCancelled;
    public event EventHandler ConnectionAttemptFailed;
    public event EventHandler ConnectionFailed;
    public event EventHandler ConnectionLost;
    public event EventHandler ConnectionClosed;

    public ClientConnectionState ConnectionState { get; }
    public int ConnectionAttempts { get; }

    public void Connect(string address, ushort port);
    public void Disconnect();
  }
}
