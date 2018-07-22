using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPStreamServer {
  class MessageHandler {
    private delegate void handleMessage(long connectionId, byte[] data);
    private static Dictionary<long, handleMessage> handlers = new Dictionary<long, handleMessage>();
    private static long packetLength;

    public static void init() {
      long TESTING = 1;
      handlers.Add(TESTING, TEST_HANDLER);
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
      handleMessage handler;

      buffer.writeBytes(data);
      packetId = buffer.readLong();
      buffer.Dispose();

      if (handlers.TryGetValue(packetId, out handler)) {
        handler.Invoke(clientId, data);
      }

    }

    private static void TEST_HANDLER(long clientId, byte[] data) {
      ByteBuffer buffer = new ByteBuffer();
      buffer.writeBytes(data);

      long messageType = buffer.readLong();

      // read other crap from the buffer.

    }

  }
}