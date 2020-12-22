using CommonClasses.Services.Interfaces;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Exceptions;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonClasses.Services
{
  public abstract class BaseClientService : IClientService
  {
    private const int MAX_CONNECTION_ATTEMPTS = 0;
    private const int RETRY_DELAY = 100;

    public event EventHandler ConnectionSuccessful;
    public event EventHandler ConnectionCancelled;
    public event EventHandler ConnectionAttemptFailed;
    public event EventHandler ConnectionFailed;
    public event EventHandler ConnectionLost;
    public event EventHandler ConnectionClosed;

    public ClientConnectionState ConnectionState { get; private set; }
    public int ConnectionAttempts { get; private set; }

    protected IMqttClient Client { get; private set; }

    private IMqttClientOptions _options;
    private string _address;
    private ushort _port;


    public BaseClientService()
    {
      ConnectionState = ClientConnectionState.Disconnected;
      Client = null;
      _options = null;
      _address = null;
      _port = default;
      ConnectionAttempts = default;
    }


    public async void Connect(string address, ushort port)
    {
      // Argument Validation //
      if (string.IsNullOrWhiteSpace(address))
        throw new ArgumentException("Must not be null or only whitespace", nameof(address));

      // State Validation //
      if (ConnectionState != ClientConnectionState.Disconnected)
        throw new InvalidOperationException("Client is not disconnected.");

      // MQTT Client Construction //
      var factory = new MqttFactory();
      Client = factory.CreateMqttClient();

      // Client Options Construction //
      _options = new MqttClientOptionsBuilder()
        .WithTcpServer(address, port)
        .Build();

      // Store Arguments //
      _address = address;
      _port = port;

      // Client Event Handlers //
      Client.UseConnectedHandler(Client_ConnectedHandler);
      Client.UseDisconnectedHandler(Client_DisconnectedHandler);

      // Attempt Connections //
      ConnectionState = ClientConnectionState.Connecting;
      ConnectionAttempts = 0;
      while (ConnectionState == ClientConnectionState.Connecting)
      {
        // Attempt Connection, break if successful.
        var successful = await AttemptConnection(_options);
        if (successful) break;

        // Break if no longer connecting.
        if (ConnectionState != ClientConnectionState.Connecting)
          break;

        // DEBUG: Log connection retrying.
        Log.Debug($"Retrying connection to ('{_address}:{_port}') in {RETRY_DELAY} milliseconds.");

        // Wait <RETRY_DELAY> Milliseconds Before Retrying Connection
        await Task.Delay(TimeSpan.FromMilliseconds(RETRY_DELAY));
      }
    }


    public async void Disconnect()
    {
      // If not connected: Error
      if (ConnectionState == ClientConnectionState.Disconnected)
        throw new InvalidOperationException("Client is already disconnected");

      // Disconnect from Server
      await Client.DisconnectAsync();
    }


    /// <summary>
    /// Attempts a connection using the passed <paramref name="options"/>
    /// and increments <see cref="ConnectionAttempts"/> by 1.
    /// </summary>
    /// <returns><c>true</c> if the connection was successful, otherwise <c>false</c>.</returns>
    private Task<bool> AttemptConnection(IMqttClientOptions options)
    {
      return Task.Run(async () =>
      {
        try
        {
          ConnectionAttempts++;
          await Client.ConnectAsync(options, CancellationToken.None);
          return true;
        }
        catch (Exception ex) when (ex is MqttCommunicationException || ex is OperationCanceledException)
        { return false; }
      });
    }


    #region Callbacks

    protected virtual void OnConnectionSuccessful()
    {
      // DEBUG: Log successful connection.
      Log.Debug($"Connection to ('{_address}:{_port}') was successful after ({ConnectionAttempts}) attempt(s).");

      ConnectionState = ClientConnectionState.Connected;
      ConnectionSuccessful?.Invoke(this, new EventArgs());
    }

    protected virtual void OnConnectionCancelled()
    {
      // DEBUG: Log connection cancelled.
      Log.Debug($"Connection to ('{_address}:{_port}') was cancelled after ({ConnectionAttempts}) attempt(s).");

      ConnectionState = ClientConnectionState.Disconnected;
      ConnectionCancelled?.Invoke(this, new EventArgs());
    }

    protected virtual void OnConnectionAttemptFailed()
    {
      // DEBUG: Log failed connection attempt.
      var maxAttemptsMsg = (MAX_CONNECTION_ATTEMPTS > 0) ? $"/{MAX_CONNECTION_ATTEMPTS}" : "";
      Log.Debug($"Connection attempt({ConnectionAttempts}{maxAttemptsMsg}) to ('{_address}:{_port}') failed.");

      ConnectionAttemptFailed?.Invoke(this, new EventArgs());
    }

    protected virtual void OnConnectionFailed()
    {
      // DEBUG: Log failed connection.
      Log.Debug($"Connection to ('{_address}:{_port}') failed after ({ConnectionAttempts}) attempt(s).");

      ConnectionState = ClientConnectionState.Disconnected;
      ConnectionFailed?.Invoke(this, new EventArgs());
    }

    protected virtual void OnConnectionLost()
    {
      // DEBUG: Log connection lost.
      Log.Debug($"Connection to ('{_address}:{_port}') was lost.");

      ConnectionState = ClientConnectionState.Disconnected;
      ConnectionLost?.Invoke(this, new EventArgs());
    }

    protected virtual void OnConnectionClosed()
    {
      // DEBUG: Log connection closed.
      Log.Debug($"Connection to ('{_address}:{_port}') was closed.");

      ConnectionState = ClientConnectionState.Disconnected;
      ConnectionClosed?.Invoke(this, new EventArgs());
    }

    #endregion Callbacks


    #region Callback Handlers

    private void Client_ConnectedHandler(MqttClientConnectedEventArgs evt)
    {
      OnConnectionSuccessful();
    }

    private void Client_DisconnectedHandler(MqttClientDisconnectedEventArgs evt)
    {
      switch (ConnectionState)
      {
        case ClientConnectionState.Connected:
          // Assume connection was closed if no exception was present,
          // otherwise connection was lost.
          if (evt.Exception == null)
            OnConnectionClosed();
          else
            OnConnectionLost();
          break;

        case ClientConnectionState.Connecting:
          // Assume connection was cancelled if no exception was present,
          // otherwise connection attempt failed.
          if (evt.Exception == null)
          {
            OnConnectionCancelled();
          }
          else
          {
            OnConnectionAttemptFailed();

            // Limit Connection Attempts.
            if (MAX_CONNECTION_ATTEMPTS > 0 && ConnectionAttempts >= MAX_CONNECTION_ATTEMPTS)
              OnConnectionFailed();
          }
          break;

        default:
          throw new NotImplementedException($"A {nameof(ConnectionState)} of '{ConnectionState}' is not handled.");
      }
    }

    #endregion Callback Handlers
  }
}
