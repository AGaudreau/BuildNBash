using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rotateItems : MonoBehaviour
{

  public Button leftButton;
  public Button rightButton;

  //temporary variables for testing button use
  public GameObject cube1;
  public GameObject cube2;
  public GameObject cube3;

  // Start is called before the first frame update
  public void leftButtonClick() {
    if (cube1.active)
    {
      cube1.SetActive(false);
      cube3.SetActive(true);
    }
    else if (cube2.active)
    {
      cube2.SetActive(false);
      cube1.SetActive(true);
    }
    else
    {
      cube3.SetActive(false);
      cube2.SetActive(true);
    }
  }

  public void rightButtonClick() {
    Debug.Log(cube1.active + " " + cube2.active + " " + cube3.active);
    if (cube1.active)
    {
      cube1.SetActive(false);
      cube2.SetActive(true);
    }
    else if (cube2.active)
    {
      cube2.SetActive(false);
      cube3.SetActive(true);
    }
    else
    {
      cube3.SetActive(false);
      cube1.SetActive(true);
    }
  }
}
