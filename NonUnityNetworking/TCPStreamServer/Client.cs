using System;
using System.Net;
using System.Net.Sockets;

namespace TCPStreamServer {
  class Client {
    public const int BUFFER_SIZE = 4096;
    public int id;
    public string ip;
    public TcpClient connection;
    public NetworkStream stream;

    private byte[] readBuffer;
    public ByteBuffer buffer;

    public void Start() {
      connection.SendBufferSize = BUFFER_SIZE;
      connection.ReceiveBufferSize = BUFFER_SIZE;

      stream = connection.GetStream();
      readBuffer = new byte[BUFFER_SIZE];
      stream.BeginRead(readBuffer, 0, BUFFER_SIZE, recieveDataFromClient, null);
    }

    public void recieveDataFromClient(IAsyncResult result) {
      try {
        int readBytes = stream.EndRead(result);
        // If we got no data, return
        if (readBytes <= 0) {
          closeConnection();
          return;
        }

        byte[] newBytes = new byte[readBytes];
        Buffer.BlockCopy(readBuffer, 0, newBytes, 0, readBytes);

        MessageHandling.handleData(id, newBytes);

        // Got data from the client, start listening for more data
        stream.BeginRead(readBuffer, 0, BUFFER_SIZE, recieveDataFromClient, null);

      } catch (Exception) {
        closeConnection();
      }
    }

    private void closeConnection() {
      connection.Close();
      stream.Close();
    }

  }
}
