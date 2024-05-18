using UnityEngine;

public class RotateKart : MonoBehaviour
{
    public float rotationSpeed = 100.0f;
    private bool isDragging = false;
    private Vector3 lastPosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            // Rotar el kart basado en el último delta de entrada
            Vector3 delta = Vector3.zero;

            if (Input.GetMouseButton(0))
            {
                delta = (Vector3)Input.mousePosition - lastPosition;
            }

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    delta = (Vector3)touch.position - lastPosition;
                }
            }

            float rotationY = delta.x * rotationSpeed * Time.fixedDeltaTime;
            transform.Rotate(Vector3.up, -rotationY, Space.World);
            lastPosition += delta;
        }
    }
}

