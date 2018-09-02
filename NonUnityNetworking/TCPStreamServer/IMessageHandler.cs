using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public delegate void MessageHandler(long connectionId, IMessage msg);

public class MessageHandlers {
  List<MessageHandler> handlers = new List<MessageHandler>();

  public void AddHandler(MessageHandler msgHandler) {
    handlers.Add(msgHandler);
  }

  public void HandleMessage(long clientId, IMessage msg) {
    foreach (var item in handlers) {
      item.Invoke(clientId, msg);
    }
  }
}