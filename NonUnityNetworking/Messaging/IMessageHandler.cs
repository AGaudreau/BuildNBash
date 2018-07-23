using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class IMessageHandler {
  public delegate void MessageHandler(long connectionId, byte[] data);
  public MessageHandler MessageHandlerDelegate;

  public uint getHandlerId() {
    string handlerName = GetType().Name;
    byte[] nameAsBytes = Encoding.ASCII.GetBytes(handlerName);
    return Crc32.Compute(nameAsBytes);
  }
}