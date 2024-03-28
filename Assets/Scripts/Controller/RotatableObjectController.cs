using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatableObjectController : MonoBehaviour
{
    private Camera myCam;
    private Vector3 screenPoint;
    private float angleOffset;
    private Collider2D col;
    private float previousZRotation;

    public RotatableObject modelo;
    public RotatableObjectView vista;
    public Thermometer thermometer;


    private void Start()
    {
        myCam = Camera.main;
        col = GetComponent<Collider2D>();
        modelo = new RotatableObject();
        vista = GetComponent<RotatableObjectView>(); // Asume que la vista está en el mismo GameObject
        previousZRotation = transform.eulerAngles.z;
    }

    private void Update()
    {
        Vector3 mousePos = myCam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (col == Physics2D.OverlapPoint(mousePos))
            { 
                screenPoint = myCam.WorldToScreenPoint(transform.position);
                Vector3 vec3 = Input.mousePosition - screenPoint;
                angleOffset = (Mathf.Atan2(transform.right.y, transform.right.x) - Mathf.Atan2(vec3.y, vec3.x)) * Mathf.Rad2Deg;
                previousZRotation = transform.eulerAngles.z; // Actualiza la rotación previa en Z al comenzar a interactuar
            }
        }
        if (Input.GetMouseButton(0))
        {

            if (col == Physics2D.OverlapPoint(mousePos))
            {
                Vector3 vec3 = Input.mousePosition - screenPoint;
                float angle = Mathf.Atan2(vec3.y, vec3.x) * Mathf.Rad2Deg;
                vista.AplicarRotacion(angle, angleOffset);

                float currentZRotation = vista.transform.eulerAngles.z;
                float rotationDifference = Mathf.Abs(Mathf.DeltaAngle(currentZRotation, previousZRotation));
                modelo.UpdateRotation(rotationDifference);

                int currentFrame = modelo.GetCurrentFrame();
                currentFrame = Mathf.Clamp(currentFrame, 0, 30); // Esto asegura que el frame esté entre 0 y 30
                Debug.Log($"Total Rotation: {modelo.TotalRotation}, Current Frame: {currentFrame}");

                thermometer.SwitchSprite(currentFrame);

                previousZRotation = currentZRotation;
            }
        }
    }
}
