using System.Collections.Generic;
using UnityEngine;


// MAKE SURE TO INCLUDE THE ENUM THAT DECLARES THE PACKET TYPES

public class MessageHandling : MonoBehaviour {

  public static ByteBuffer packerBuffer;
  private static Dictionary<uint, MessageHandlers> messageHandlers = new Dictionary<uint, MessageHandlers>();
  private static Dictionary<uint, IMessageMaker> messageMakers = new Dictionary<uint, IMessageMaker>();

  private static long packetLength;

  private void Awake() {
    Init();
  }

  private static void Init() {
    RegisterMessageHandler<TestMessage>(TestMessageHandler.HandleMessage);
    RegisterMessageMaker<TestMessage>(new TestMessageMaker());
  }

  public static void RegisterMessageHandler<messageType>(MessageHandler msgHandler) {
    //Console.WriteLine("Registering handler for: " + typeof(messageType).Name);
    uint handlerId = IMessage.GetMessageId<messageType>();
    
    MessageHandlers msgHandlers;
    if (!messageHandlers.TryGetValue((uint)handlerId, out msgHandlers)) {
      //Console.WriteLine("Creating new handlers");
      msgHandlers = new MessageHandlers();
      messageHandlers.Add(handlerId, msgHandlers);
    }
    
    msgHandlers.AddHandler(msgHandler);
    //Console.WriteLine("Adding new handler delegate. \nFinished registering handler.");
  }

  public static void RegisterMessageMaker<messageType>(IMessageMaker msgMaker) {
    //Console.WriteLine("Registering maker for: " + typeof(messageType).Name);
    uint handlerId = IMessage.GetMessageId<messageType>();
    messageMakers.Add(handlerId, msgMaker);
    //Console.WriteLine("Finished registering maker.");
  }

  public static void HandleData(byte[] data) {
    byte[] buffer = (byte[])data.Clone();


    if (packerBuffer == null) {
      packerBuffer = new ByteBuffer();
    }


    packerBuffer.WriteBytes(buffer);
    if (packerBuffer.Count() == 0) {
      packerBuffer.Clear();
      return;
    }

    if (packerBuffer.Length() >= 8) {
      packetLength = packerBuffer.ReadLong(false);
      if (packetLength < 0) {
        packerBuffer.Clear();
        return;
      }
    }

    while (packetLength > 0 && packetLength <= packerBuffer.Length() - 8) {
      if (packetLength <= packerBuffer.Length() - 8) {
        packerBuffer.ReadLong();
        data = packerBuffer.ReadBytes((int)packetLength);
        HandleDataPackets(data);
      }

      if (packerBuffer.Length() >= 8) {
        packetLength = packerBuffer.ReadLong(false);
        if (packetLength < 0) {
          packerBuffer.Clear();
          return;
        }
      }
    }
  }

  private static void HandleDataPackets(byte[] data) {
    long packetId;
    ByteBuffer buffer = new ByteBuffer();
    MessageHandlers msgHandler;
    IMessageMaker msgMaker;

    buffer.WriteBytes(data);
    packetId = buffer.ReadLong();
    buffer.Dispose();

    if (messageMakers.TryGetValue((uint)packetId, out msgMaker)) {
      IMessage message = msgMaker.FromBytes(data);
      if (messageHandlers.TryGetValue((uint)packetId, out msgHandler)) {
        msgHandler.HandleMessage(message);
      } else {
        //Console.WriteLine("We recieved a message who's handler was not registered.");
      }
    } else {
      //Console.WriteLine("We recieved a message who's maker was not registered.");
    }
  }
}
