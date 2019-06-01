using UnityEngine.UI;
using UnityEngine;

public class Cube_Interaction : MonoBehaviour
{
  void HitByRay() {
    Debug.Log("Button Pressed");
    GetComponent<Button>().onClick.Invoke();
  }
}
