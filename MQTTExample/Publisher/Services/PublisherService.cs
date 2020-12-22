using CommonClasses.Services;
using Publisher.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Publisher.Services
{
  public class PublisherService : BaseClientService, IPublisherService
  {
    public void Publish(string topic, string payload)
    {
      throw new NotImplementedException();
    }
  }
}
