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

        // This waits for a key to be pressed in the console, we might want to change this in the future?
        if (Console.ReadKey().Key == ConsoleKey.Escape)
          break;


        if (Console.ReadKey().Key == ConsoleKey.M) {
          stream.send(0, new TestMessage("This Is A Test: If this were a real message, you would probably do something."));
        }
      }
    }
  }
}
