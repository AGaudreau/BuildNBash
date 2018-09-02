using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class TestMessageHandler {
  public static void HandleMessage(IMessage msg) {
    TestMessage testMessage = (TestMessage)msg; // This is casting and might be slow.



    TestMessage response = new TestMessage("I see your test message and raise you one test message.");

    Client.instance.SendMessage(response);

    //Console.WriteLine(testMessage.testMessageContent);
  }
}