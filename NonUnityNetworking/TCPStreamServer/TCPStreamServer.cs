using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TCPStreamServer {
  class TCPStreamServer {
    public const int MAX_CLIENTS = 100;
    static public Client[] clients = new Client[MAX_CLIENTS];

    static void Main(string[] args) {

      MessageHandling.init();
      TCPStream stream = new TCPStream(clients);
      int connectionPort = 5555;
      stream.startStream(connectionPort);

      Array.Clear(clients, 0, clients.Length);

      // Main server loop
      while (true) {
        


        if (Console.ReadKey().Key == ConsoleKey.Escape)
          break;
      }
    }
  }
}
