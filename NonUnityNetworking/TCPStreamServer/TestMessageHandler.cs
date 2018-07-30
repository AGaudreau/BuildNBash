using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class TestMessageHandler {
  public static void handleMessage(long connectionId, IMessage msg) {
    TestMessage testMessage = (TestMessage)msg; // This is casting and might be slow.
    Console.WriteLine(testMessage.testMessageContent);
  }
}
