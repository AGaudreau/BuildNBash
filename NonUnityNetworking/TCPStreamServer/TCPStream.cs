using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace TCPStreamServer {
  class TCPStream {
    public TcpListener streamSocket;
    public static Client[] clients;

    //private Dictionary messagePackers = new Dictionary<long, >()

    public TCPStream(Client[] clientsArray) {
      clients = clientsArray;
    }

    public void StartStream(int streamPort) {
      streamSocket = new TcpListener(IPAddress.Any, streamPort);
      streamSocket.Start();
      streamSocket.BeginAcceptTcpClient(AcceptedTcpClient, null);
    }

    public void AcceptedTcpClient(IAsyncResult result) {
      TcpClient connectedClient = streamSocket.EndAcceptTcpClient(result);
      Console.WriteLine("Client " + connectedClient.Client.RemoteEndPoint.ToString() + " connected.");

      // After accepting the client connection, start listening for more client connections
      streamSocket.BeginAcceptTcpClient(AcceptedTcpClient, null);

      for (int i = 0; i < clients.Length; ++i) {
        if (clients[i] == null) {
          clients[i] = new Client();
          clients[i].connection = connectedClient;
          clients[i].id = i;
          clients[i].ip = connectedClient.Client.RemoteEndPoint.ToString();
          clients[i].Start();
          return;
        }
      }

      Console.WriteLine("ERROR: Out of client connections!!!");
    }


    private void SendDataTo(int clientId, byte[] data) {
      ByteBuffer buffer = new ByteBuffer();
      buffer.WriteLong(data.GetUpperBound(0 - data.GetLowerBound(0)) + 1);
      buffer.WriteBytes(data);
      clients[clientId].stream.BeginWrite(buffer.ToArray(), 0, buffer.ToArray().Length, null, null);
    }


    public void SendMessage(int clientId, IMessage messageToSend) {
      SendDataTo(clientId, messageToSend.ToBytes());
    }
  }
}
