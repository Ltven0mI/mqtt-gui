using CommonClasses.Services.Interfaces;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Publisher.Services.Interfaces
{
  /// <summary>
  /// Supports publishing of messages to a connected MQTT server.
  /// </summary>
  public interface IPublisherService : IClientService
  {
    /// <summary>
    /// Invoked when a message is successfully published.
    /// </summary>
    public event EventHandler<MessagePublishedEventArgs> MessagePublished;

    /// <summary>
    /// Publish a message to the connected MQTT server.
    /// </summary>
    /// <param name="topic">The message topic.</param>
    /// <param name="payload">The message payload.</param>
    public void Publish(string topic, string payload);
  }

  /// <summary>
  /// Represents the data of a published message.
  /// </summary>
  public class MessagePublishedEventArgs : EventArgs
  {
    public string Topic { get; set; }
    public string Payload { get; set; }
  }
}
