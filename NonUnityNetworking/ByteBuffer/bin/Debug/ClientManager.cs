using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour {
  private Client client;

  void Start() {
    DontDestroyOnLoad(this);
    client = new Client();
    client.connect();
  }

  private void OnApplicationQuit() {
    client.connection.Close();
  }

  // Update is called once per frame
  void Update() {

  }
}
