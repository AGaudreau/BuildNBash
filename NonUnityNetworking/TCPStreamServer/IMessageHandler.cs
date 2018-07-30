using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public delegate void MessageHandler(long connectionId, IMessage msg);

public class IMessageHandlers {
  List<MessageHandler> handlers = new List<MessageHandler>();

  public void addHandler(MessageHandler msgHandler) {
    handlers.Add(msgHandler);
  }

  public void handleMessage(long clientId, IMessage msg) {
    foreach (var item in handlers) {
      item.Invoke(clientId, msg);
    }
  }
}