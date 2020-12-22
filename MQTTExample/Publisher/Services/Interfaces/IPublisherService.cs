using CommonClasses.Services.Interfaces;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Publisher.Services.Interfaces
{
    public interface IPublisherService : IClientService
    {
        public void Publish(string topic, string payload);
    }
}
