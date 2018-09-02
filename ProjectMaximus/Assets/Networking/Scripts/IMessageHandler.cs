using System.Collections.Generic;

public delegate void MessageHandler(IMessage msg);

public class MessageHandlers {
  List<MessageHandler> handlers = new List<MessageHandler>();

  public void AddHandler(MessageHandler msgHandler) {
    handlers.Add(msgHandler);
  }

  public void HandleMessage(IMessage msg) {
    foreach (var item in handlers) {
      item.Invoke(msg);
    }
  }
}