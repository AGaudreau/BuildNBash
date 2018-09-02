using System;

class TestMessageHandler {
  public static void HandleMessage(long connectionId, IMessage msg) {
    TestMessage testMessage = (TestMessage)msg; // This is casting and might be slow.
    Console.WriteLine(testMessage.testMessageContent);
  }
}
