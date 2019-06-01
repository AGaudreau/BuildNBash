using UnityEngine.UI;
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
        //Ray tabletRay = tabletCamera.ViewportPointToRay(hit.textureCoord);
        Ray tabletRay = tabletCamera.ScreenPointToRay(new Vector2(localPoint.x * tabletCamera.pixelWidth, localPoint.y * tabletCamera.pixelHeight));
        RaycastHit tabletHit;

          if (Physics.Raycast(tabletRay, out tabletHit)) {
            Debug.Log(tabletHit.collider.gameObject.name);
            tabletHit.transform.SendMessage("HitByRay");
          }

        }
      }
    }
}
