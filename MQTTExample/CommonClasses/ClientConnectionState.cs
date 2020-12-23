using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClasses
{
  /// <summary>
  /// Represents the connection state of an MQTT client.
  /// </summary>
  public enum ClientConnectionState { None, Disconnected, Connecting, Connected }
}
