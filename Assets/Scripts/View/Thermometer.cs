using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thermometer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer donde se mostrar� el term�metro
    public Sprite[] sprites; // Array de sprites para los diferentes estados del term�metro

    private void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>(); // Aseg�rate de tener un SpriteRenderer
    }

    public void SwitchSprite(int frame)
    {
        // Asegurarte de que el frame est� en el rango
        if (frame < 0 || frame >= 30)
        {
            Debug.LogWarning("La llanta se sobrecalento por la fricci�n: " + frame);
            return;
        }

        // Mapear el frame a los sprites, teniendo en cuenta que hay 2 sprites por frame
        // y que la asignaci�n est� invertida (frame 0 corresponde al sprite m�s alto).
        int spriteIndex = sprites.Length - 1 - (frame * 2);

        // Si el spriteIndex calculado es impar y sobrepasa el l�mite del array,
        // ajustar para usar el �ltimo sprite disponible.
        if (spriteIndex >= sprites.Length)
        {
            spriteIndex = sprites.Length - 1;
        }

        spriteRenderer.sprite = sprites[spriteIndex];
    }
}
