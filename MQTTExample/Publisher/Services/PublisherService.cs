using CommonClasses.Services;
using MQTTnet;
using Publisher.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Publisher.Services
{
  public class PublisherService : BaseClientService, IPublisherService
  {
    public event EventHandler<MessagePublishedEventArgs> MessagePublished;

    public async void Publish(string topic, string payload)
    {
      var payloadBytes = Encoding.UTF8.GetBytes(payload);

      var message = new MqttApplicationMessageBuilder()
        .WithTopic(topic)
        .WithPayload(payloadBytes)
        .WithExactlyOnceQoS()
        .Build();

      await Client.PublishAsync(message, CancellationToken.None);

      OnMessagePublished(topic, payload);
    }


    #region Callbacks

    private void OnMessagePublished(string topic, string payload)
    {
      MessagePublished?.Invoke(this, new MessagePublishedEventArgs() { Topic=topic, Payload=payload });
    }

    #endregion Callbacks
  }
}
