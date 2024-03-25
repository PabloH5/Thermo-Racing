using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    //public Thermometer thermometer;
    private Camera myCam;
    private Vector3 screenPoint;
    private float angleOffset;
    private Collider2D col;
    private float previousZRotation; // Almacena la rotaci�n en Z del frame anterior
    private float totalRotation = 0f; // Acumulador de la rotaci�n total

    private void Start()
    {
        myCam = Camera.main;
        col = GetComponent<Collider2D>();
        previousZRotation = transform.eulerAngles.z; // Inicializa con la rotaci�n actual en Z
    }

    private void Update()
    {
        Vector3 mousePos = myCam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if(col == Physics2D.OverlapPoint(mousePos))
            {
                //Debug.Log(Physics2D.OverlapPoint(mousePos));
                screenPoint = myCam.WorldToScreenPoint(transform.position);
                Vector3 vec3 = Input.mousePosition - screenPoint;
                angleOffset = (Mathf.Atan2(transform.right.y, transform.right.x) - Mathf.Atan2(vec3.y, vec3.x)) * Mathf.Rad2Deg;
                previousZRotation = transform.eulerAngles.z; // Actualiza la rotaci�n previa en Z al comenzar a interactuar
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (col == Physics2D.OverlapPoint(mousePos))
            {
                Vector3 vec3 = Input.mousePosition - screenPoint;
                float angle = Mathf.Atan2(vec3.y, vec3.x) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, 0, angle + angleOffset);

                // Calcula la diferencia de rotaci�n respecto al frame anterior
                float currentZRotation = transform.eulerAngles.z;
                float rotationDifference = Mathf.Abs(Mathf.DeltaAngle(currentZRotation, previousZRotation));

                // Acumula la rotaci�n total
                totalRotation += rotationDifference;

                Debug.Log(totalRotation);
                // Actualiza previousZRotation para el pr�ximo frame
                previousZRotation = currentZRotation;

                // Calcula el n�mero de frame actual basado en la rotaci�n total
                int currentFrame = Mathf.FloorToInt(totalRotation / 360f);
                currentFrame = Mathf.Clamp(currentFrame, 0, 30); // Asegura que el frame est� entre 0 y 30
                
                Debug.Log(currentFrame);

                // Aqu� puedes implementar la l�gica para actualizar tu term�metro
                // Por ejemplo: thermometer.Update(totalRotation);
            }
        }
    }
}