using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelToInflate : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private List<Sprite> spritesWH;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SwitchSprite(int spritePos)
    {
        if (spritePos != 4)
        {
            spriteRenderer.sprite = spritesWH[spritePos];
        }
        else
        {
            spriteRenderer.color = Color.red;
        }

    }
}
