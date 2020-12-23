using CommonClasses.Services;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Exceptions;
using Serilog;
using Subscriber.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Subscriber.Services
{
  public class SubscriberService : BaseClientService, ISubscriberService
  {
    public event EventHandler<SubscriptionEventArgs> Subscribed;
    public event EventHandler<SubscriptionEventArgs> Unsubscribed;
    public event EventHandler<MessageReceivedEventArgs> MessageReceived;


    public override void Connect(string address, ushort port)
    {
      base.Connect(address, port);
      Client.UseApplicationMessageReceivedHandler(Client_ApplicationMessageReceivedHandler);
    }

    public async void Subscribe(string topic)
    {
      var result = await Client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());
      Log.Debug("Subscribed result: {@result}", result);
      OnSubscribed(topic);
    }

    public void Unsubscribe(string topic)
    {
      var result = Client.UnsubscribeAsync(new string[] { topic });
      Log.Debug("Unsubscribed result {@result}", result);
      OnUnsubscribed(topic);
    }


    #region Callbacks

    private void OnSubscribed(string topic)
    {
      Subscribed?.Invoke(this, new SubscriptionEventArgs() { Topic = topic });
    }

    private void OnUnsubscribed(string topic)
    {
      Unsubscribed?.Invoke(this, new SubscriptionEventArgs() { Topic = topic });
    }

    private void OnMessageReceived(string topic, string payload)
    {
      MessageReceived?.Invoke(this, new MessageReceivedEventArgs() { Topic = topic, Payload = payload });
    }

    #endregion Callbacks


    #region Callback Handlers

    private void Client_ApplicationMessageReceivedHandler(MqttApplicationMessageReceivedEventArgs evt)
    {
      var topic = evt.ApplicationMessage.Topic;
      var payloadBytes = evt.ApplicationMessage.Payload;
      var payload = payloadBytes != null ? Encoding.UTF8.GetString(payloadBytes) : null;
      OnMessageReceived(topic, payload);
    }

    #endregion Callback Handlers
  }
}
