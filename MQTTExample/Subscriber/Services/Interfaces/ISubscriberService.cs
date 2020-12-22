using CommonClasses.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscriber.Services.Interfaces
{
  public interface ISubscriberService : IClientService
  {
    public void Subscribe(string topic);
    public void Unsubscribe(string topic);
  }
}
