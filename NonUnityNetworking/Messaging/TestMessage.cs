using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TestMessage: IMessage {

  public string testMessageContent;

  public TestMessage() {
    
  }

  public TestMessage(string testData) {
    testMessageContent = testData;
  }

  public override byte[] toBytes() {
    ByteBuffer messageBuffer = new ByteBuffer();

    messageBuffer.writeLong(IMessage.getMessageId<TestMessage>());
    messageBuffer.writeString(testMessageContent);

    return messageBuffer.toArray();
  }
}

public class TestMessageMaker: IMessageMaker {
  public override IMessage fromBytes(byte[] data) {
    ByteBuffer messageBuffer = new ByteBuffer();
    messageBuffer.writeBytes(data);
    long msgTypeWeAlreadyKnow = messageBuffer.readLong(); // We need to read to move the reading pointer

    return new TestMessage(messageBuffer.readString()); 
  }
}