using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletInteraction : MonoBehaviour
{
  public Camera tabletCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    if (Input.GetMouseButtonDown(0)) {
        RaycastHit hit;
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

      if (Physics.Raycast(ray, out hit)) {
          Debug.Log(hit.collider.gameObject);

          var localPoint = hit.textureCoord;
          Ray tabletRay = tabletCamera.ViewportPointToRay(hit.textureCoord);
          RaycastHit tabletHit;

          if (Physics.Raycast(tabletRay, out tabletHit)) {
            Debug.Log(tabletHit.collider.gameObject.name);
          }

        }
      }
    }
}
