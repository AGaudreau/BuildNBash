using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Utility;

public class rotateItems : MonoBehaviour
{

  //temporary variables for testing button use
  public GameObject cube1;
  public GameObject cube2;
  public GameObject cube3;

  public GameObject spawnSphere;

  public Camera inventoryCamera;
  public Camera robotCamera;
  public Camera shopCamera;


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

  public void inventorySwitch() {
    Transform robotParent = (Transform)robotCamera.transform.parent;
    robotParent.gameObject.SetActive(false);

    Transform shopParent = (Transform)shopCamera.transform.parent;
    shopParent.gameObject.SetActive(false);

    Transform inventoryParent = (Transform)inventoryCamera.transform.parent;
    inventoryParent.gameObject.SetActive(true);
  }

  public void shopSwitch() {
    Transform robotParent = (Transform)robotCamera.transform.parent;
    robotParent.gameObject.SetActive(false);

    Transform inventoryParent = (Transform)inventoryCamera.transform.parent;
    inventoryParent.gameObject.SetActive(false);

    Transform shopParent = (Transform)shopCamera.transform.parent;
    shopParent.gameObject.SetActive(true);
  }

  public void robotSwitch() {
    Transform shopParent = (Transform)shopCamera.transform.parent;
    shopParent.gameObject.SetActive(false);

    Transform inventoryParent = (Transform)inventoryCamera.transform.parent;
    inventoryParent.gameObject.SetActive(false);

    Transform robotParent = (Transform)robotCamera.transform.parent;
    robotParent.gameObject.SetActive(true);
  }

  public void selectRobot() {
    if (cube1.active)
    {
      GameObject cube1Clone = (GameObject)Instantiate(cube1, spawnSphere.transform.position, spawnSphere.transform.rotation);
      Destroy(cube1Clone.GetComponent<UnityStandardAssets.Utility.AutoMoveAndRotate>());
      Transform property = cube1Clone.GetComponent<Transform>();
      Vector3 scale = new Vector3(.15f, .15f, .15f);
      property.localScale = scale;
    }
    else if (cube2.active)
    {
      GameObject cube2Clone = (GameObject)Instantiate(cube2, spawnSphere.transform.position, spawnSphere.transform.rotation);
      Destroy(cube2Clone.GetComponent<UnityStandardAssets.Utility.AutoMoveAndRotate>());
      Transform property = cube2Clone.GetComponent<Transform>();
      Vector3 scale = new Vector3(.15f, .15f, .15f);
      property.localScale = scale;
    }
    else if (cube3.active)
    {
      GameObject cube3Clone = (GameObject)Instantiate(cube3, spawnSphere.transform.position, spawnSphere.transform.rotation);
      Destroy(cube3Clone.GetComponent<UnityStandardAssets.Utility.AutoMoveAndRotate>());
      Transform property = cube3Clone.GetComponent<Transform>();
      Vector3 scale = new Vector3(.15f, .15f, .15f);
      property.localScale = scale;
    }
  }
}
