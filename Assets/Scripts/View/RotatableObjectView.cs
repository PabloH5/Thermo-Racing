using UnityEngine;

public class RotatableObjectView : MonoBehaviour
{
    public void AplicarRotacion(float angle, float angleOffset)
    {
        transform.eulerAngles = new Vector3(0, 0, angle + angleOffset);
    }

    // Aqu� puedes a�adir m�s l�gica de la vista como cambiar sprites si es necesario
}
