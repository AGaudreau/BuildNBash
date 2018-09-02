using System;
using System.Net.Sockets;
using System.Net;
using UnityEngine;


class Client : MonoBehaviour {

  public static Client instance;

  static int BUFFER_SIZE = 4096;
  public TcpClient connection;
  public NetworkStream stream;
  private byte[] asyncBuffer;
  public bool isConnected;

  public byte[] recieveBytes;
  public bool handleData = false;

  private string IP_ADDRESS = "127.0.0.1"; // Local host
  private int PORT = 5555;

  private void Awake() {
    instance = this;
  }

  private void Update() {
    if (handleData == true) {
      MessageHandling.handleData(recieveBytes);
      handleData = false;
    }
  }

  public void connect() {
    Debug.Log("Attempting to connect to server");

    connection = new TcpClient();
    connection.ReceiveBufferSize = BUFFER_SIZE;
    connection.SendBufferSize = BUFFER_SIZE;
    asyncBuffer = new byte[BUFFER_SIZE * 2];

    try {
      connection.BeginConnect(IP_ADDRESS, PORT, new AsyncCallback(onConnectionEstablished), connection);
    } catch (Exception) {
      // TODO tell the user
    }
  }

  private void onConnectionEstablished(IAsyncResult result) {
    try {
      connection.EndConnect(result);

      // If connection failed
      if (connection.Connected == false) {
        // TODO tell the user
        isConnected = false;
        return;
      } else {
        stream = connection.GetStream();
        stream.BeginRead(asyncBuffer, 0, BUFFER_SIZE * 2, onReceiveData, null);
        isConnected = true;
        Debug.Log("Connected to server");
      }

    } catch (Exception) {
      // TODO tell the user
      isConnected = false;
      return;
    }
  }

  private void onReceiveData(IAsyncResult result) {
    try {
      int packetLength = stream.EndRead(result);
      recieveBytes = new byte[packetLength];
      Buffer.BlockCopy(asyncBuffer, 0, recieveBytes, 0, packetLength);

      if (packetLength == 0) {
        Debug.Log("Disconnected.");
        Application.Quit();
        return;
      }

      handleData = true;
      stream.BeginRead(asyncBuffer, 0, 8192, onReceiveData, null);

    } catch (Exception) {
      Debug.Log("Disconnected.");
      Application.Quit();
      return;
    }
  }

  private void sendData(byte[] data) {
    ByteBuffer buffer = new ByteBuffer();
    buffer.writeLong(data.GetUpperBound(0) - data.GetLowerBound(0) + 1);
    buffer.writeBytes(data);
    stream.Write(buffer.toArray(), 0, buffer.toArray().Length);
  }

  public void send(IMessage messageToSend) {
    sendData(messageToSend.toBytes());
  }
}
