using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    private void OnMouseDown() {
        var mouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(mouse);
    }

    private void OnMouseDrag() {
        var curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        var curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        curPosition.z = 0f;
        transform.position = curPosition;
    }
}
