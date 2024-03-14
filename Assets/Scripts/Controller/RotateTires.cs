using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTires : MonoBehaviour
{
   public float rotationSpeed = 100f; // Velocidad de rotación en grados por segundo

    void Update()
    {
        // Rotar la llanta alrededor del eje Z
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
