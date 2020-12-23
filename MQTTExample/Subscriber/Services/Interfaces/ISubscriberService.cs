using CommonClasses.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscriber.Services.Interfaces
{
  /// <summary>
  /// Supports subscribing / unsubscribing from topics on a connected MQTT server.
  /// </summary>
  public interface ISubscriberService : IClientService
  {
    /// <summary>
    /// Invoked when a topic is successfully subscribed to.
    /// </summary>
    public event EventHandler<SubscriptionEventArgs> Subscribed;

    /// <summary>
    /// Invoked when a topic is successfully unsubscribed from.
    /// </summary>
    public event EventHandler<SubscriptionEventArgs> Unsubscribed;

    /// <summary>
    /// Invoked when a message is received from the connected MQTT server.
    /// </summary>
    public event EventHandler<MessageReceivedEventArgs> MessageReceived;

    /// <summary>
    /// Subscribe to a topic on the connected MQTT server.
    /// </summary>
    /// <param name="topic">The topic to subscribe to.</param>
    public void Subscribe(string topic);

    /// <summary>
    /// Unsubscribe from a topic on the connected MQTT server.
    /// </summary>
    /// <param name="topic">The topic to unsubscribe from.</param>
    public void Unsubscribe(string topic);
  }


  /// <summary>
  /// Represents a topic that has been subscribed to, or unsubscribed from.
  /// </summary>
  public class SubscriptionEventArgs : EventArgs
  {
    public string Topic { get; set; }
  }

  /// <summary>
  /// Represents the data of a received message.
  /// </summary>
  public class MessageReceivedEventArgs : EventArgs
  {
    public string Topic { get; set; }
    public string Payload { get; set; }
  }
}
