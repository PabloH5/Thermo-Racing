using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableItem : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                offset = gameObject.transform.position - hit.point;
            }
        }
        
        if (isDragging)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                gameObject.transform.position = new Vector3(hit.point.x + offset.x, hit.point.y + offset.y, gameObject.transform.position.z);
            }
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}
