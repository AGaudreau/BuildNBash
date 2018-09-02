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

  public override byte[] ToBytes() {
    ByteBuffer messageBuffer = new ByteBuffer();

    messageBuffer.WriteLong(IMessage.GetMessageId<TestMessage>());
    messageBuffer.WriteString(testMessageContent);

    return messageBuffer.ToArray();
  }
}

public class TestMessageMaker: IMessageMaker {
  public override IMessage FromBytes(byte[] data) {
    ByteBuffer messageBuffer = new ByteBuffer();
    messageBuffer.WriteBytes(data);
    long msgTypeWeAlreadyKnow = messageBuffer.ReadLong(); // We need to read to move the reading pointer

    return new TestMessage(messageBuffer.ReadString()); 
  }
}