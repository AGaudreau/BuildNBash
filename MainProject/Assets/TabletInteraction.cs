using UnityEngine.UI;
using UnityEngine;

public class TabletInteraction : MonoBehaviour
{
  public Camera robotCamera;
  public Camera invetoryCamera;
  public Camera ShopCamera;

    // Update is called once per frame
    void Update()
    {
    if (Input.GetMouseButtonDown(0)) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

      if (robotCamera.isActiveAndEnabled)
      {
        activeCameraForRaycast(robotCamera, ray);
      }
      else if (invetoryCamera.isActiveAndEnabled)
      {
        activeCameraForRaycast(invetoryCamera, ray);
      }
      else if (ShopCamera.isActiveAndEnabled) {
        activeCameraForRaycast(ShopCamera, ray);
      }


    }

  void activeCameraForRaycast(Camera activeCamera, Ray ray)
    {
      RaycastHit hit;
      if (Physics.Raycast(ray, out hit))
      {
        //Debug.Log(hit.collider.gameObject);

        var localPoint = hit.textureCoord;
        //Ray tabletRay = tabletCamera.ViewportPointToRay(hit.textureCoord);
        Ray tabletRay = activeCamera.ScreenPointToRay(new Vector2(localPoint.x * activeCamera.pixelWidth, localPoint.y * activeCamera.pixelHeight-64));
        RaycastHit tabletHit;
        
        if (Physics.Raycast(tabletRay, out tabletHit))
        {
          //Debug.Log(tabletHit.collider.gameObject.name);
          tabletHit.transform.SendMessage("HitByRay");
        }

      }
    }
  }
}
