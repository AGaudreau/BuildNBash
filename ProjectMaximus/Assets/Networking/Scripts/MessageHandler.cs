using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// MAKE SURE TO INCLUDE THE ENUM THAT DECLARES THE PACKET TYPES

public class MessageHandler : MonoBehaviour {

  public static ByteBuffer packerBuffer;
  private delegate void Packet_(byte[] data);
  private static Dictionary<long, Packet_> handlers = new Dictionary<long, Packet_>();
  private static long packetLength;

  private void Awake() {
    init();
  }

  private static void init() {

    handlers.Add(IMessage.getMessageId<TestMessage>(), receiveTesting);
  }

  public static void handleData(byte[] data) {
    byte[] buffer = (byte[])data.Clone();


    if (packerBuffer == null) {
      packerBuffer = new ByteBuffer();
    }


    packerBuffer.writeBytes(buffer);
    if (packerBuffer.count() == 0) {
      packerBuffer.clear();
      return;
    }

    if (packerBuffer.length() >= 8) {
      packetLength = packerBuffer.readLong(false);
      if (packetLength < 0) {
        packerBuffer.clear();
        return;
      }
    }

    while (packetLength > 0 && packetLength <= packerBuffer.length() - 8) {
      if (packetLength <= packerBuffer.length() - 8) {
        packerBuffer.readLong();
        data = packerBuffer.readBytes((int)packetLength);
        handleDataPackets(data);
      }

      if (packerBuffer.length() >= 8) {
        packetLength = packerBuffer.readLong(false);
        if (packetLength < 0) {
          packerBuffer.clear();
          return;
        }
      }
    }
  }

  private static void handleDataPackets(byte[] data) {
    long packetId;
    ByteBuffer buffer = new ByteBuffer();
    Packet_ packet;

    buffer.writeBytes(data);
    packetId = buffer.readLong();
    buffer.Dispose();

    if (handlers.TryGetValue(packetId, out packet)) {
      packet.Invoke(data);
    }

  }

  private static void receiveTesting(byte[] data) {
    ByteBuffer buffer = new ByteBuffer();
    buffer.writeBytes(data);

    long packetIdentifier = buffer.readLong();

    string messageData = buffer.readString();
    int testing = messageData.Length;
    buffer.clear();

    TestMessage response = new TestMessage("I see your test message and raise you one test message.");

    buffer.writeBytes(response.toBytes());
    Client.instance.sendData(buffer.toArray());
  }

}
