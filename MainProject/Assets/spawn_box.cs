using System.Collections.Generic;
using UnityEngine;

public class spawn_box : MonoBehaviour
{
  public GameObject fillerObject;

  private int count = 1;

  public void OnMouseDown() {
    //Debug.Log("Spawn Box");

    GameObject box = (GameObject)Instantiate(fillerObject);
    //Vector3 scale = new Vector3(.25f, .25f, .25f);
    //box.transform.localScale = scale;
    Vector3 position = new Vector3(0f, 2f, 1f);
    box.transform.position = position;
    box.name = fillerObject.name + count++;
  }
}
