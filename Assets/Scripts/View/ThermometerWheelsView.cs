using System.Collections.Generic;
using UnityEngine;

public class ThermometerWheelsView : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private List<Sprite> spritesTH;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SwitchSprite(int spritePos)
    {
        spriteRenderer.sprite = spritesTH[spritePos];
    }

}
