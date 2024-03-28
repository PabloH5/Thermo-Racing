using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatableObjectView : MonoBehaviour
{
    public void AplicarRotacion(float angle, float angleOffset)
    {
        transform.eulerAngles = new Vector3(0, 0, angle + angleOffset);
    }

    // Aquí puedes añadir más lógica de la vista como cambiar sprites si es necesario
}
