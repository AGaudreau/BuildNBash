using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{

  Vector3 topPoint;
  Vector3 bottomPoint;
  Vector3 leftPoint;
  Vector3 rightPoint;
  Vector3 frontPoint;
  Vector3 backPoint;


  public float radius = .5f;
  public float halfWidth = .08f;
  private Collider[] hitCollider;
  
  bool collision = false;


  void OnDrawGizmos()
  {
    if (collision)
    {
      Gizmos.color = Color.red;
    }
    else {
      Gizmos.color = Color.blue;
    }
    Gizmos.DrawWireSphere(transform.position, radius);

    Gizmos.color = Color.yellow;
    Gizmos.DrawLine(transform.position, topPoint);
    Gizmos.color = Color.magenta;
    Gizmos.DrawLine(transform.position, bottomPoint);
    Gizmos.color = Color.green;
    Gizmos.DrawLine(transform.position, frontPoint);
    Gizmos.color = Color.cyan;
    Gizmos.DrawLine(transform.position, backPoint);
    Gizmos.color = Color.black;
    Gizmos.DrawLine(transform.position, leftPoint);
    Gizmos.color = Color.red;
    Gizmos.DrawLine(transform.position, rightPoint);
  }

  private void LateUpdate()
  {
    topPoint = transform.position + Vector3.Normalize(transform.TransformVector(Vector3.up)) * halfWidth;
    bottomPoint = transform.position + Vector3.Normalize(transform.TransformVector(Vector3.down)) * halfWidth;
    frontPoint = transform.position + Vector3.Normalize(transform.TransformVector(Vector3.forward)) * halfWidth;
    backPoint = transform.position + Vector3.Normalize(transform.TransformVector(Vector3.back)) * halfWidth;
    rightPoint = transform.position + Vector3.Normalize(transform.TransformVector(Vector3.right)) * halfWidth;
    leftPoint = transform.position + Vector3.Normalize(transform.TransformVector(Vector3.left)) * halfWidth;
  }

  private void Update()
  {
    //Debug.Log("Selected object Id: " + ID);
    hitCollider = Physics.OverlapSphere(transform.position, radius);
    foreach (Collider item in hitCollider)
    {
      if (item.transform.parent)
      {
        if (item.transform.parent.name.Contains("Block") && item.transform.parent.name != transform.name)
        {
          //Debug.Log("Name of collision object: " + item.transform.parent.name 
          // + " Name of selected object: " + transform.name);
          collision = true;
        }
      }
      else
      {
        collision = false;
      }
    }
  }

void grabbed()
  {
    if (collision) {
      
    }
  }

}
