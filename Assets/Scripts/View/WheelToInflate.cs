using System.Collections.Generic;
using UnityEngine;

public class WheelToInflate : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private List<Sprite> spritesWH;

    public List<Sprite> SpritesWH
    {
        get { return spritesWH; }
        set { spritesWH = value; }
    }
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = SpritesWH[0];
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
