using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPStreamServer {
  class TCPStreamServer {
    static void Main(string[] args) {
      // Main server loop
      while(true) {


        TCPStream stream = new TCPStream();
        stream.startStream(5555);
        
        if (Console.ReadKey().Key == ConsoleKey.Escape)
          break;
      }
    }
  }
}
