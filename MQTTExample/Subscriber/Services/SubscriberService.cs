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
    public void Subscribe(string topic)
    {
      throw new NotImplementedException();
      //await Client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());
    }

    public void Unsubscribe(string topic)
    {
      throw new NotImplementedException();
    }
  }
}
