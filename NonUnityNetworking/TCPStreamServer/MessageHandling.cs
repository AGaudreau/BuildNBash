using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPStreamServer {

  class MessageHandling {

    private static Dictionary<uint, IMessageMaker> messageMakers = new Dictionary<uint, IMessageMaker>();
    private static Dictionary<uint, MessageHandlers> messageHandlers = new Dictionary<uint, MessageHandlers>();
    private static long packetLength;

    public static void Init() {
      RegisterMessageHandler<TestMessage>(TestMessageHandler.HandleMessage);
      RegisterMessageMaker<TestMessage>(new TestMessageMaker());
    }

    public static void RegisterMessageMaker<messageType>(IMessageMaker msgMaker) {
      Console.WriteLine("Registering maker for: " + typeof(messageType).Name);
      uint handlerId = IMessage.GetMessageId<messageType>();
      messageMakers.Add(handlerId, msgMaker);
      Console.WriteLine("Finished registering maker.");
    }

    public static void RegisterMessageHandler<messageType>(MessageHandler msgHandler) {
      Console.WriteLine("Registering handler for: " + typeof(messageType).Name);
      uint handlerId = IMessage.GetMessageId<messageType>();

      MessageHandlers msgHandlers;
      if (!messageHandlers.TryGetValue((uint)handlerId, out msgHandlers)) {
        Console.WriteLine("Creating new handlers");
        msgHandlers = new MessageHandlers();
        messageHandlers.Add(handlerId, msgHandlers);
      }

      msgHandlers.AddHandler(msgHandler);
      Console.WriteLine("Adding new handler delegate. \nFinished registering handler.");

    }

    public static void HandleData(long clientId, byte[] data) {
      byte[] dataBuffer = (byte[])data.Clone();

      if (TCPStream.clients[clientId].buffer == null) {
        TCPStream.clients[clientId].buffer = new ByteBuffer();
      }

      ByteBuffer clientBuffer = TCPStream.clients[clientId].buffer;
      clientBuffer.WriteBytes(dataBuffer);

      if (clientBuffer.Count() == 0) {
        clientBuffer.Clear();
        return;
      }

      if (clientBuffer.Length() >= 4) {
        packetLength = clientBuffer.ReadLong(false);
        if (packetLength <= 0) {
          clientBuffer.Clear();
          return;
        }
      }

      while (packetLength > 0 && packetLength <= clientBuffer.Length() - 8) {
        if (packetLength <= clientBuffer.Length() - 8) {
          clientBuffer.ReadLong();
          data = clientBuffer.ReadBytes((int)packetLength);
          HandleDataPackets(clientId, data);
        }

        packetLength = 0;

        if (clientBuffer.Length() >= 4) {
          packetLength = clientBuffer.ReadLong(false);
          
          if (packetLength <= 0) {
            clientBuffer.Clear();
            return;
          }
        }

        if (packetLength <= 1) {
          clientBuffer.Clear();
        }
      }
    }

    private static void HandleDataPackets(long clientId, byte[] data) {
      long packetId;
      ByteBuffer buffer = new ByteBuffer();
      MessageHandlers msgHandler;
      IMessageMaker msgMaker;

      buffer.WriteBytes(data);
      packetId = buffer.ReadLong();
      buffer.Dispose();
            
      if (messageMakers.TryGetValue((uint)packetId, out msgMaker)) {
        IMessage message = msgMaker.FromBytes(data);
        if (messageHandlers.TryGetValue((uint)packetId, out msgHandler)) {
          msgHandler.HandleMessage(clientId, message);
        } else {
          Console.WriteLine("We recieved a message who's handler was not registered.");
        }
      } else {
        Console.WriteLine("We recieved a message who's maker was not registered.");
      }
    }
  }
}