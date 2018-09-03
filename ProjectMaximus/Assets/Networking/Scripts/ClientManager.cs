using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientManager : MonoBehaviour {

  public Button connectButton;
  public Button disconnectButton;

  public Text ipAddress;
  public Text port;

  void Start() {
    DontDestroyOnLoad(this);
    connectButton.onClick.AddListener(ConnectButton_OnClicked);
    disconnectButton.onClick.AddListener(DisconnectButton_OnClicked);
  }

  private void OnApplicationQuit() {
    Client.instance.connection.Close();
  }

  // Update is called once per frame
  void Update() {
    connectButton.enabled = !Client.instance.isConnected;
    disconnectButton.enabled = Client.instance.isConnected;
    ipAddress.enabled = !Client.instance.isConnected;
    port.enabled = !Client.instance.isConnected;
  }

  void ConnectButton_OnClicked() {
    int portNumber = int.Parse(port.text);
    Client.instance.Connect(ipAddress.text, portNumber);
  }

  void DisconnectButton_OnClicked() {
    Client.instance.Disconnect();
  }
}
