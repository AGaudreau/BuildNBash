using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class IMessage {
  public abstract byte[] toBytes();

  public static uint getMessageId<messageType>() {
    string handlerName = typeof(messageType).Name;
    byte[] nameAsBytes = Encoding.ASCII.GetBytes(handlerName);
    return Crc32.Compute(nameAsBytes);
  }
}

public abstract class IMessageMaker {
  public abstract IMessage fromBytes(byte[] data);
}