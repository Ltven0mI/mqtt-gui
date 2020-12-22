using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher.Services.Interfaces
{
  public interface IMQTTClient
  {
    #region Events

    #endregion Events

    public void Connect(string address, ushort port);
  }
}
