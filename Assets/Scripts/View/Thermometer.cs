using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thermometer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer donde se mostrará el termómetro
    public Sprite[] sprites; // Array de sprites para los diferentes estados del termómetro

    private void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>(); // Asegúrate de tener un SpriteRenderer
    }

    public void SwitchSprite(int frame)
    {
        // Asegurarte de que el frame esté en el rango
        if (frame < 0 || frame >= 30)
        {
            Debug.LogWarning("La llanta se sobrecalento por la fricción: " + frame);
            return;
        }

        // Mapear el frame a los sprites, teniendo en cuenta que hay 2 sprites por frame
        // y que la asignación está invertida (frame 0 corresponde al sprite más alto).
        int spriteIndex = sprites.Length - 1 - (frame * 2);

        // Si el spriteIndex calculado es impar y sobrepasa el límite del array,
        // ajustar para usar el último sprite disponible.
        if (spriteIndex >= sprites.Length)
        {
            spriteIndex = sprites.Length - 1;
        }

        spriteRenderer.sprite = sprites[spriteIndex];
    }
}
