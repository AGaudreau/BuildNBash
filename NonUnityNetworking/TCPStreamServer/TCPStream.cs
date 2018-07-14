using System;
using System.Net;
using System.Net.Sockets;

namespace TCPStreamServer {
  class TCPStream {
    public const int MAX_CLIENTS = 100;

    public TcpListener streamSocket;

    public ClientData[] clients = new ClientData[MAX_CLIENTS];

    public void startStream(int streamPort) {
      streamSocket = new TcpListener(IPAddress.Any, streamPort);
      streamSocket.Start();
      streamSocket.BeginAcceptTcpClient(acceptedTcpClient, null);
    }

    public void acceptedTcpClient(IAsyncResult result) {
      TcpClient connectedClient = streamSocket.EndAcceptTcpClient(result);
      Console.WriteLine("Client " + connectedClient.Client.RemoteEndPoint.ToString() + " connected.");

      // After accepting the client connection, start listening for more client connections
      streamSocket.BeginAcceptTcpClient(acceptedTcpClient, null);

      for (int i = 0; i < clients.Length; ++i) {
        if (clients[i] == null) {
          clients[i].connection = connectedClient;
          clients[i].id = i;
          clients[i].ip = connectedClient.Client.RemoteEndPoint.ToString();
          clients[i].Start();
        }
        return;
      }

      Console.WriteLine("ERROR: Out of client connections!!!");


    }



  }
}
