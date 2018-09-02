using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour {

  void Start() {
    DontDestroyOnLoad(this);
    Client.instance.Connect();
  }

  private void OnApplicationQuit() {
    Client.instance.connection.Close();
  }

  // Update is called once per frame
  void Update() {

  }
}
