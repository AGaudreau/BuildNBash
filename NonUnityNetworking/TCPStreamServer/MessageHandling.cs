using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPStreamServer {

  // Test message handler
  public class TestMessageHandler : IMessageHandler {
    public TestMessageHandler() {
      MessageHandlerDelegate = handleMessage;
    }
    public static void handleMessage(long connectionId, byte[] data) {
      Console.WriteLine("TestMessageHandler: Message received from (" + connectionId.ToString() + " ).");
    }
  }

  class MessageHandling {
    private static Dictionary<uint, IMessageHandler> messageHandlers = new Dictionary<uint, IMessageHandler>();
    private static long packetLength;

    public static void init() {
      registerMessageHandler(new TestMessageHandler());
    }

    public static void registerMessageHandler(IMessageHandler handler) {
      uint handlerId = handler.getHandlerId();
      messageHandlers.Add(handlerId, handler);
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
      IMessageHandler handler;

      buffer.writeBytes(data);
      packetId = buffer.readLong();
      buffer.Dispose();

      if (messageHandlers.TryGetValue((uint)packetId, out handler)) {
        handler.MessageHandlerDelegate.Invoke(clientId, data);
      }
    }
  }
}