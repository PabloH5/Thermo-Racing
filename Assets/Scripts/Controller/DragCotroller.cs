using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragCotroller : MonoBehaviour
{
    private bool isDragActive = false;
    private Vector2 screenPos;
    private Vector3 worldPos;

    private DraggableItem lastDrag;
    void Awake()
    {
        DragCotroller[] controllers = FindObjectsOfType<DragCotroller>();
        if (controllers.Length > 1)
        {
            Destroy(gameObject);
        }

    }
    void Update()
    {
        if (isDragActive)
        {
            if (Input.GetMouseButton(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                Drop();
                return;
            }
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            screenPos = new Vector2(mousePos.x, mousePos.y);
        }
        else if (Input.touchCount > 0)
        {
            screenPos = Input.GetTouch(0).position;
        }
        else { return; }

        worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        if (isDragActive)
        {
            Drag();
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            if (hit.collider != null)
            {
                Debug.Log("nasheeeee");
                DraggableItem draggableItem = hit.transform.gameObject.GetComponent<DraggableItem>();
                if (draggableItem != null)
                {
                    lastDrag = draggableItem;
                    Debug.Log("nashe");
                    InitDrag();
                }
            }
        }
    }
    void InitDrag()
    {
        isDragActive = true;
    }

    void Drag()
    {
        lastDrag.transform.position = new Vector2(worldPos.x, worldPos.y);
        Debug.Log("a");
    }
    void Drop()
    {
        isDragActive = false;
    }
}
