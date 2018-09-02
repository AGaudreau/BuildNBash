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

    public static void init() {
      registerMessageHandler<TestMessage>(TestMessageHandler.handleMessage);
      registerMessageMaker<TestMessage>(new TestMessageMaker());
    }

    public static void registerMessageMaker<messageType>(IMessageMaker msgMaker) {
      Console.WriteLine("Registering maker for: " + typeof(messageType).Name);
      uint handlerId = IMessage.getMessageId<messageType>();
      messageMakers.Add(handlerId, msgMaker);
      Console.WriteLine("Finished registering maker.");
    }

    public static void registerMessageHandler<messageType>(MessageHandler msgHandler) {
      Console.WriteLine("Registering handler for: " + typeof(messageType).Name);
      uint handlerId = IMessage.getMessageId<messageType>();

      MessageHandlers msgHandlers;
      if (!messageHandlers.TryGetValue((uint)handlerId, out msgHandlers)) {
        Console.WriteLine("Creating new handlers");
        msgHandlers = new MessageHandlers();
        messageHandlers.Add(handlerId, msgHandlers);
      }

      msgHandlers.addHandler(msgHandler);
      Console.WriteLine("Adding new handler delegate. \nFinished registering handler.");

    }

    public static void handleData(long clientId, byte[] data) {
      byte[] dataBuffer = (byte[])data.Clone();

      if (TCPStream.clients[clientId].buffer == null) {
        TCPStream.clients[clientId].buffer = new ByteBuffer();
      }

      ByteBuffer clientBuffer = TCPStream.clients[clientId].buffer;
      clientBuffer.writeBytes(dataBuffer);

      if (clientBuffer.count() == 0) {
        clientBuffer.clear();
        return;
      }

      if (clientBuffer.length() >= 4) {
        packetLength = clientBuffer.readLong(false);
        if (packetLength <= 0) {
          clientBuffer.clear();
          return;
        }
      }

      while (packetLength > 0 && packetLength <= clientBuffer.length() - 8) {
        if (packetLength <= clientBuffer.length() - 8) {
          clientBuffer.readLong();
          data = clientBuffer.readBytes((int)packetLength);
          handleDataPackets(clientId, data);
        }

        packetLength = 0;

        if (clientBuffer.length() >= 4) {
          packetLength = clientBuffer.readLong(false);
          
          if (packetLength <= 0) {
            clientBuffer.clear();
            return;
          }
        }

        if (packetLength <= 1) {
          clientBuffer.clear();
        }
      }
    }

    private static void handleDataPackets(long clientId, byte[] data) {
      long packetId;
      ByteBuffer buffer = new ByteBuffer();
      MessageHandlers msgHandler;
      IMessageMaker msgMaker;

      buffer.writeBytes(data);
      packetId = buffer.readLong();
      buffer.Dispose();
            
      if (messageMakers.TryGetValue((uint)packetId, out msgMaker)) {
        IMessage message = msgMaker.fromBytes(data);
        if (messageHandlers.TryGetValue((uint)packetId, out msgHandler)) {
          msgHandler.handleMessage(clientId, message);
        } else {
          Console.WriteLine("We recieved a message who's handler was not registered.");
        }
      } else {
        Console.WriteLine("We recieved a message who's maker was not registered.");
      }
    }
  }
}