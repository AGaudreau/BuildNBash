using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragScript : MonoBehaviour
{
  public float reach = 2;

  private void OnMouseDrag() {
    Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, reach);
    Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
    transform.position = objPosition;
  }

  // Update is called once per frame
  void Update()
  {
    
  }
}
