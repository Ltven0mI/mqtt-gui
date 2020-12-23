using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClasses.Services.Interfaces
{
  /// <summary>
  /// Supports connecting to and disconnecting from a MQTT server.
  /// </summary>
  public interface IClientService
  {
    /// <summary>
    /// Invoked when the client succesfully connects to an MQTT server.
    /// </summary>
    public event EventHandler ConnectionSuccessful;

    /// <summary>
    /// Invoked when the connection is cancelled while connecting.
    /// </summary>
    public event EventHandler ConnectionCancelled;

    /// <summary>
    /// Invoked when a connection attempt fails.
    /// </summary>
    public event EventHandler ConnectionAttemptFailed;

    /// <summary>
    /// Invoked when the maximum number of connection attempts is reached.
    /// </summary>
    public event EventHandler ConnectionFailed;

    /// <summary>
    /// Invoked when the connection to the connected MQTT server is lost.
    /// </summary>
    public event EventHandler ConnectionLost;

    /// <summary>
    /// Invoked when the connection to the connected MQTT server is closed.
    /// </summary>
    public event EventHandler ConnectionClosed;

    /// <summary>
    /// The current connection state.
    /// </summary>
    public ClientConnectionState ConnectionState { get; }

    /// <summary>
    /// The current connection attempt, will increase with every attempt.
    /// </summary>
    public int ConnectionAttempts { get; }

    /// <summary>
    /// Connect to an MQTT server.
    /// </summary>
    /// <param name="address">The server's address.</param>
    /// <param name="port">The server's port.</param>
    public void Connect(string address, ushort port);

    /// <summary>
    /// Close the connection to the currently connected MQTT server,
    /// or cancel the current connection attempt.
    /// </summary>
    public void Disconnect();
  }
}
