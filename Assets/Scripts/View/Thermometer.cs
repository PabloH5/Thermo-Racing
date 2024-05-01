using UnityEngine;

public class Thermometer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

    private void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SwitchSprite(int frame)
    {
        if (frame < 0 || frame >= 30)
        {
            Debug.LogWarning("La llanta se sobrecalento por la friccion: " + frame);
            return;
        }

        int spriteIndex = sprites.Length - 1 - (frame * 2);

        if (spriteIndex >= sprites.Length)
        {
            spriteIndex = sprites.Length - 1;
        }

        spriteRenderer.sprite = sprites[spriteIndex];
    }
}
