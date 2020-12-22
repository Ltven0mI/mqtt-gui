using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Publisher.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Publisher.Services
{
  public class MQTTClient : IMQTTClient
  {
    public async void Connect(string address, ushort port)
    {
      // Create Factory and Client
      var factory = new MqttFactory();
      var mqttClient = factory.CreateMqttClient();

      // Create Options
      var options = new MqttClientOptionsBuilder()
        .WithTcpServer(address, port)
        .Build();

      // Connect Client to Server
      await mqttClient.ConnectAsync(options, CancellationToken.None);

      // Create Disconnected Handler
      mqttClient.UseDisconnectedHandler(async e =>
      {
        MessageBox.Show("### DISCONNECTED FROM SERVER ###");
        await Task.Delay(TimeSpan.FromSeconds(5));

        try
        {
          await mqttClient.ConnectAsync(options, CancellationToken.None);
        }
        catch
        {
          MessageBox.Show("### RECONNECTING FAILED ###");
        }
      });

      // Publish Message
      var message = new MqttApplicationMessageBuilder()
        .WithTopic("house/livingroom/temperature")
        .WithPayload("297.15")
        .WithExactlyOnceQoS()
        .Build();

      await mqttClient.PublishAsync(message, CancellationToken.None);
    }
  }
}
