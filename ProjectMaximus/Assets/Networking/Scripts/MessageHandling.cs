using System.Collections.Generic;
using UnityEngine;


// MAKE SURE TO INCLUDE THE ENUM THAT DECLARES THE PACKET TYPES

public class MessageHandling : MonoBehaviour {

  public static ByteBuffer packerBuffer;
  private static Dictionary<uint, MessageHandlers> messageHandlers = new Dictionary<uint, MessageHandlers>();
  private static Dictionary<uint, IMessageMaker> messageMakers = new Dictionary<uint, IMessageMaker>();

  private static long packetLength;

  private void Awake() {
    init();
  }

  private static void init() {
    registerMessageHandler<TestMessage>(TestMessageHandler.handleMessage);
    registerMessageMaker<TestMessage>(new TestMessageMaker());
  }

  public static void registerMessageHandler<messageType>(MessageHandler msgHandler) {
    //Console.WriteLine("Registering handler for: " + typeof(messageType).Name);
    uint handlerId = IMessage.getMessageId<messageType>();
    
    MessageHandlers msgHandlers;
    if (!messageHandlers.TryGetValue((uint)handlerId, out msgHandlers)) {
      //Console.WriteLine("Creating new handlers");
      msgHandlers = new MessageHandlers();
      messageHandlers.Add(handlerId, msgHandlers);
    }
    
    msgHandlers.addHandler(msgHandler);
    //Console.WriteLine("Adding new handler delegate. \nFinished registering handler.");
  }

  public static void registerMessageMaker<messageType>(IMessageMaker msgMaker) {
    //Console.WriteLine("Registering maker for: " + typeof(messageType).Name);
    uint handlerId = IMessage.getMessageId<messageType>();
    messageMakers.Add(handlerId, msgMaker);
    //Console.WriteLine("Finished registering maker.");
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
    MessageHandlers msgHandler;
    IMessageMaker msgMaker;

    buffer.writeBytes(data);
    packetId = buffer.readLong();
    buffer.Dispose();

    if (messageMakers.TryGetValue((uint)packetId, out msgMaker)) {
      IMessage message = msgMaker.fromBytes(data);
      if (messageHandlers.TryGetValue((uint)packetId, out msgHandler)) {
        msgHandler.handleMessage(message);
      } else {
        //Console.WriteLine("We recieved a message who's handler was not registered.");
      }
    } else {
      //Console.WriteLine("We recieved a message who's maker was not registered.");
    }
  }
}
